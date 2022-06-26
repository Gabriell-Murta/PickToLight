#pragma once

#include <ArduinoJson.h>
#include "CommandType.h"

class DeviceCommand {
 public:
  CommandType type;
  std::string deviceId;
  std::string timestamp;
};

bool convertToJson(const DeviceCommand& command, JsonVariant json) {
  json["type"] = command.type;
  json["deviceId"] = command.deviceId;
  json["timestamp"] = command.timestamp;

  return true;
}

void convertFromJson(JsonVariantConst json, DeviceCommand& command) {
  command.type = json["type"].as<CommandType>();
  command.deviceId.assign(json["deviceId"].as<String>().c_str());
  command.timestamp.assign(json["timestamp"].as<String>().c_str());
}
