package config

import (
	"encoding/json"
	"log"
	"os"
)

type testConfig struct {
	BaseUrl      string
	SessionToken string
}

func NewTestConfig() *testConfig {
	return loadConfig()
}

func loadConfig() *testConfig {
	content, err := os.ReadFile("../../test-config.json")
	if err != nil {
		log.Fatal(err)
	}

	var config testConfig
	err = json.Unmarshal(content, &config)
	if err != nil {
		log.Fatal(err)
	}

	return &config
}
