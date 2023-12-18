export interface Ruleset{
  id: number;
  name: string;
  description: string;
  isEnabled: boolean;
  waterConsumptionThreshold: number;
  isIrrigationDurationEnabled: boolean;
  maxIrrigationDuration: {
    hours: number;
    minutes: number;
    seconds: number;
  };
  rainAvoidanceEnabled: boolean;
  rainProbabilityThreshold: number;
  rainAmountThreshold: number;
  humidityGrowthPerRainMm: number;
  rainAvoidanceDurationThreshold: {
    hours: number;
    minutes: number;
    seconds: number;
  };
  temperatureThreshold: number;
  minSoilHumidityThreshold: number;
  optimalSoilHumidity: number;
  waterConsumptionPerMinute: number;
  humidityGrowthPerLiterConsumed: number;
}
