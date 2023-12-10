#include <WiFi.h>
#include <WiFiClient.h>
#include <WebServer.h>
#include <uri/UriBraces.h>

#include <Adafruit_Sensor.h>
#include "DHT.h"

#include <ESP32Servo.h>

#define DHT_PIN 15
#define DHTTYPE DHT22

#define SERVO_PIN 32

#define WIFI_SSID "Wokwi-GUEST"
#define WIFI_PASSWORD ""
// Defining the WiFi channel speeds up the connection:
#define WIFI_CHANNEL 6

DHT dht(DHT_PIN, DHTTYPE);
WebServer server(80);
Servo servo;

void sendResponse(bool isSuccess)
{
    server.send(200, "application/json", isSuccess ? "{\"IsSuccess\":true}" : "{\"IsSuccess\":false}");
    
}

void readDhtData(String* data)
{
    float h = dht.readHumidity();

    float t = dht.readTemperature();

    if (isnan(h) || isnan(t))
    {
        *data = "null";
    }

    *data = "{\"temperature\":" + String(t,2) + ",\"humidity\":" + String(h, 2) + "}";
}

void startIrrigation(int* duration){
    Serial.println("Starting irrigation");
    servo.write(180);  
    Serial.println("Irrigation finished");
}

void stopIrrigation(){
    Serial.println("Stopping irrigation");  
    
    servo.write(-90);    
    delay(100);    
    Serial.println("Irrigation stopped");
}

void setup(void)
{
    Serial.begin(9600);
    dht.begin();  

    servo.attach(SERVO_PIN); 
    servo.write(-90);   

    WiFi.begin(WIFI_SSID, WIFI_PASSWORD, WIFI_CHANNEL);
    Serial.print("Connecting to WiFi ");
    Serial.print(WIFI_SSID);
    // Wait for connection
    while (WiFi.status() != WL_CONNECTED)
    {
        delay(100);
        Serial.print(".");
    }
    Serial.println(" Connected!");

    Serial.print("IP address: ");
    Serial.println(WiFi.localIP());

    server.on(UriBraces("/get-data"), []()
    {
        String data;
        readDhtData(&data);
        server.send(200, "application/json", data);
    });

    server.on(UriBraces("/start-irrigation/{}/{}"), []()
    {
        String pathArg = server.pathArg(0);
        int duration = pathArg.toInt();
        startIrrigation(&duration);
        sendResponse(true);
    });

    server.on(UriBraces("/stop-irrigation"), []()
    {
        stopIrrigation();
        sendResponse(true);
    });

    server.begin();
    Serial.println("HTTP server started (http://localhost:8180)");
}

void loop(void)
{
    server.handleClient();
    delay(2);
}


