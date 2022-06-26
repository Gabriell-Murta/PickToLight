#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <string>

class CredentialsRequest {
 public:
  std::string ssid;
  std::string password;
};

void convertFromJson(JsonVariantConst json, CredentialsRequest& request) {
  request.ssid.assign(json["ssid"].as<String>().c_str());
  request.password.assign(json["password"].as<String>().c_str());
}
