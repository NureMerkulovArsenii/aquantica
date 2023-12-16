#include <WiFiClientSecure.h>
#include <ESPAsyncWebServer.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_I2CDevice.h>
#include <ESP32Servo.h>
#include "DHT.h"
#include <EEPROM.h>
#include <time.h>
#include <sys/time.h>
#include <SPI.h>

#define DHT_PIN 15
#define DHTTYPE DHT22
#define SERVO_PIN 32
#define BUTTON_PIN 26
#define LED_PIN 14

#define WIFI_SSID "Wokwi-GUEST"
#define WIFI_PASSWORD ""

const char* root_ca =
"-----BEGIN CERTIFICATE-----\n"
"MIIDLzCCAhcCFDgQQukVtr250xGvky+Xf69cEBHOMA0GCSqGSIb3DQEBCwUAMFQx/\n"
"CzAJBgNVBAYTAnVhMRAwDgYDVQQIDAdVa3JhaW5lMRAwDgYDVQQHDAdLaGFya2l2\n"
"MQ0wCwYDVQQKDARudXJlMRIwEAYDVQQLDAlwenBpLTIxLTMwHhcNMjMxMjE1MjI1\n"
"MzM5WhcNMjQxMjE0MjI1MzM5WjBUMQswCQYDVQQGEwJ1YTEQMA4GA1UECAwHVWty\n"
"YWluZTEQMA4GA1UEBwwHS2hhcmtpdjENMAsGA1UECgwEbnVyZTESMBAGA1UECwwJ\n"
"cHpwaS0yMS0zMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2IPUCage\n"
"jho7w4L/rFXpSPl5tEC2qAXt/xP4lI+eHCyn9QuiP9cZC0BEQw5//Zlrwc1Ba8Vy\n"
"+HBE6hJNGjgpXQrMPpPK3GrF+Sd+iALx18cFhyH8hn3MO9gwrvlTAgUoGN4CRa0E\n"
"tOpW4YSkypiZMwooSTd1//pQ4yldwFXJWhWGUGfpN4y+XrcdqeCP5m4Yx6xxaYu2\n"
"mp/oc1hqRDx5dfjK5sAObiE75kL76pyhn7zzZB0gZWXIAYrCoLxrtwz+ViVXUPll\n"
"JORWtkVXEuwtHtWl54bcyntNIBfQ2SdgntTw3dEyxO8ELWCxdB7sD/c5jpx6SQVQ\n"
"ZzkkGwLJGG9RQQIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQCDrEuTQ3sqkFOiL7TD\n"
"E2gjGgjWA4ZUwlRnE4PMAXtw3zpPuOW1BIsOVTuKTNIiSbqg0K5eM7/c5Qt6/xBW\n"
"y0JNxvEr/grBjEuIPUgk4JaeaOmFtztgfLSlWYbNT6aGwArzb/7Iu8lqm5gA0ETU\n"
"+0n5EEIFZtTjDCVS35PkCJ1ZMqNQeURe+bwg7kzFqR26un6diJSMnTDaxDxDfAuT\n"
"PO+VdENs8zNeC2U5Bx9iUpGXNd7i8Izg5kYJR5FWWomj+BIbj3veODXYBCRkvSi6\n"
"h1Ewjlze5xo98RNDbRiOH1eF4QOlIh2k537cuUuCVgHDdbnY+LeaTlTiq9YaUGze\n"
"+Jsx\n"
"-----END CERTIFICATE-----\n";

String apiKey = "";

bool isAdmin = false;
bool ledState = false;

AsyncWebServer server(80);
DHT dht(DHT_PIN, DHTTYPE);
Servo servo;
time_t now;
WiFiClientSecure client;

void setup(void)
{
  Serial.begin(9600);
  pinMode(LED_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT_PULLUP);

  dht.begin();
  now = time(0);

  servo.attach(SERVO_PIN);
  servo.write(-90);

  //client.setCACert(root_ca);

  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  Serial.print("Connecting to WiFi ");
  Serial.print(WIFI_SSID);

  while (WiFi.status() != WL_CONNECTED)
  {
    delay(100);
    Serial.print(".");
  }
  Serial.println(" Connected!");

  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());

  // xTaskCreatePinnedToCore(
  //     task1,   /* Function to implement the task */
  //     "Task1", /* Name of the task */
  //     10000,   /* Stack size in words */
  //     NULL,    /* Task input parameter */
  //     1,       /* Priority of the task */
  //     NULL,    /* Task handle. */
  //     1);

  server.on("/admin", HTTP_GET, [](AsyncWebServerRequest* request)
    {
      if (!isAdmin) {
        request->send(403, "application/json", "{\"success\":false}");
        return;
      }

      if (request->hasHeader("x-api-key")) {
        AsyncWebHeader* h = request->getHeader("x-api-key");
        apiKey = h->value();
        request->send(200, "application/json", "{\"success\":true}");
        Serial.println("New API key: " + apiKey);
      }
      else {
        request->send(400, "application/json", "{\"success\":false}");
      }

    });

  server.on("/get-data", HTTP_GET, [](AsyncWebServerRequest* request)
    {
      if (!isApiKeyValid(request, apiKey)) {
        request->send(403, "application/json", "{\"success\":false}");
        return;
      }

      String data;
      readDhtData(&data);
      request->send(200, "application/json", data);
    });

  server.on("/start-irrigation", HTTP_GET, [](AsyncWebServerRequest* request)
    {
      if (!isApiKeyValid(request, apiKey)) {
        request->send(403, "application/json", "{\"success\":false}");
        return;
      }

      if (request->hasArg("duration"))
      {
        int duration = request->arg("duration").toInt();
        startIrrigation(duration);
        request->send(200, "application/json", "{\"success\":true}");
      }
      else
      {
        request->send(400, "application/json", "{\"success\":false}");
      }
    }
  );

  server.on("/stop-irrigation", HTTP_GET, [](AsyncWebServerRequest* request)
    {
      if (!isApiKeyValid(request, apiKey)) {
        request->send(403, "application/json", "{\"success\":false}");
        return;
      }

      stopIrrigation();
      request->send(200, "application/json", "{\"success\":true}");
    });

  server.on("/set-datetime", HTTP_POST, [](AsyncWebServerRequest* request)
    {
      if (!isApiKeyValid(request, apiKey)) {
        request->send(403, "application/json", "{\"success\":false}");
      }

      if (request->contentType() == "application/json") {
        String payload = request->getParam(0)->value();
        setDateTime(payload);
        request->send(200, "application/json", "{\"success\":true}");
      }
      else {
        request->send(400, "application/json", "{\"success\":false}");
      }
    });


  server.begin();
  Serial.println("HTTP server started (http://localhost:8180)");
}

void loop(void)
{
  //if button is pressed, toggle admin mode
  if (digitalRead(BUTTON_PIN) == LOW)
  {
    Serial.println("Button pressed");
    buttonPressed();
  }

  delay(1000);
}

void buttonPressed()
{
  isAdmin = !isAdmin;
  Serial.println("Admin mode: " + String(isAdmin));
  if (isAdmin)
  {
    digitalWrite(LED_PIN, HIGH);
  }
  else
  {
    digitalWrite(LED_PIN, LOW);
  }
}

bool isApiKeyValid(AsyncWebServerRequest* request, String apiKey) {
  if (request->hasHeader("x-api-key")) {
    AsyncWebHeader* h = request->getHeader("x-api-key");
    if (h->value() == apiKey) {
      return true;
    }
  }
  return false;
}

void readDhtData(String* data)
{
  float humidity = dht.readHumidity();
  float temperature = dht.readTemperature();

  if (isnan(humidity) || isnan(temperature))
  {
    *data = "null";
  }

  *data = "{\"temperature\":" + String(temperature, 2) + ",\"humidity\":" + String(humidity, 2) + "}";
}

void startIrrigation(int duration)
{
  Serial.println("Starting irrigation");
  servo.write(180);
  Serial.println("Irrigation finished");
}

void stopIrrigation()
{
  Serial.println("Stopping irrigation");
  servo.write(-90);
  delay(100);
  Serial.println("Irrigation stopped");
}

void task1(void* parameter)
{
  while (true)
  {
    time_t now = time(0);
    tm* localtm = localtime(&now);
    printf("The local date and time is: %s", asctime(localtm));

    Serial.println("Task 1");
    delay(1000);
  }
}

void setDateTime(String datetime)
{
  struct tm tm_struct;
  sscanf(datetime.c_str(), "%d-%d-%dT%d:%d:%d", &tm_struct.tm_year, &tm_struct.tm_mon, &tm_struct.tm_mday, &tm_struct.tm_hour, &tm_struct.tm_min, &tm_struct.tm_sec);
  tm_struct.tm_year -= 1900;
  tm_struct.tm_mon--;

  time_t new_time = mktime(&tm_struct);
  struct timeval tv = { .tv_sec = new_time };
  settimeofday(&tv, nullptr);
}