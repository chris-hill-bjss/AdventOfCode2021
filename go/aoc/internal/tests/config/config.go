package config

import (
	"encoding/json"
	"log"
	"os"
)

type TestConfig struct {
	BaseUrl      string
	SessionToken string
}

func LoadConfig() *TestConfig {
	content, err := os.ReadFile("../../test-config.json")
	if err != nil {
		log.Fatal(err)
	}

	var config TestConfig
	err = json.Unmarshal(content, &config)
	if err != nil {
		log.Fatal(err)
	}

	return &config
}
