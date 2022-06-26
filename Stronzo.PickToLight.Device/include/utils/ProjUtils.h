#pragma once

#include <Arduino.h>

#define MQTT_PING_INTERVAL 10000
#define CONFIG_DOC_SIZE 1024

const char* CONFIG_DIR = "/config.json";

static bool restart = false;
