package dayone_test

import (
	"encoding/json"
	"log"
	"os"
	"testing"

	dayOne "aoc/internal/01_dayone"
	"aoc/internal/adventclient"
)

type TestConfig struct {
	BaseUrl      string
	SessionToken string
}

const success = "\u2713"
const failure = "\u2717"

const partOneSampleInput = `199
200
208
210
200
207
240
269
260
263`

func TestDayOne(t *testing.T) {
	t.Log("Running tests for PartOne")
	{
		t.Logf("\t\twith sample input")
		{
			dayOne := dayOne.NewDayOne([]byte(partOneSampleInput))
			actual := dayOne.RunPartOne()

			assert(7, actual, t)
		}
		t.Logf("\t\twith actual input")
		{
			config := getConfig()
			client := adventclient.NewAdventClient(config.BaseUrl, config.SessionToken)

			input, err := client.GetInput(1)
			if err != nil {
				t.Fatalf("\t\t%s\tfailed to read input from AoC", failure)
			}

			dayOne := dayOne.NewDayOne([]byte(input))
			actual := dayOne.RunPartOne()

			assert(1215, actual, t)
		}
	}
}

func assert(expected int, actual int, t *testing.T) {
	if actual != expected {
		t.Fatalf("\t\t%s\texpected %v, got %v", failure, expected, actual)
	}

	t.Logf("\t\t%s\tshould return %v", success, expected)
}

func getConfig() TestConfig {
	content, err := os.ReadFile("../tests/test-config.json")
	if err != nil {
		log.Fatal(err)
	}

	var config TestConfig
	err = json.Unmarshal(content, &config)
	if err != nil {
		log.Fatal(err)
	}

	return config
}

// func readConfigFile(filename string) []byte {
// 	content, err := os.ReadFile("test-config.json")
// 	if err != nil {
// 		log.Panic(err)
// 	}

// 	return content
// }
