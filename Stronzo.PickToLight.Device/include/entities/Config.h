#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>

#include "utils/StringUtils.h"

class Config {
 public:
  // Device data
  char deviceId[37];     // guid
  char deviceToken[37];  // guid

  // WIFI STATION
  char wifiSsid[64];
  char wifiPass[64];

  // CONFIG STATUS
  bool isConfigValid = false;
};

bool convertToJson(const Config& config, JsonVariant json) {
  json["wifiSsid"] = config.wifiSsid;
  json["wifiPass"] = config.wifiPass;

  json["deviceId"] = config.deviceId;
  json["deviceToken"] = config.deviceToken;

  json["isConfigValid"] = config.isConfigValid;

  return true;
}

void convertFromJson(JsonVariantConst json, Config& config) {
  strlcpy(config.wifiSsid, json["wifiSsid"].as<String>().c_str(), sizeof(config.wifiSsid));
  strlcpy(config.wifiPass, json["wifiPass"].as<String>().c_str(), sizeof(config.wifiPass));

  strlcpy(config.deviceId, json["deviceId"].as<String>().c_str(), sizeof(config.deviceId));
  strlcpy(config.deviceToken, json["deviceToken"].as<String>().c_str(), sizeof(config.deviceToken));

  config.isConfigValid = json["isConfigValid"].as<bool>();
}
