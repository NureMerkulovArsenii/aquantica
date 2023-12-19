export interface Ruleset {
  id: number;
  name: string;
  description: string;
  isEnabled: boolean;
  waterConsumptionThreshold: number;
  isIrrigationDurationEnabled: boolean;
  maxIrrigationDuration: Date;
  rainAvoidanceEnabled: boolean;
  rainProbabilityThreshold: number;
  rainAmountThreshold: number;
  humidityGrowthPerRainMm: number;
  rainAvoidanceDurationThreshold: Date;
  temperatureThreshold: number;
  minSoilHumidityThreshold: number;
  optimalSoilHumidity: number;
  waterConsumptionPerMinute: number;
  humidityGrowthPerLiterConsumed: number;
}
