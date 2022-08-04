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

const sampleInput = `199
200
208
210
200
207
240
269
260
263`

func TestPartOne(t *testing.T) {
	t.Log("Running tests for PartOne")
	{
		partOneSample(t)
		partOneActual(t)
	}
}

func TestPartTwo(t *testing.T) {
	t.Log("Running tests for PartTwo")
	{
		partTwoSample(t)
		partTwoActual(t)
	}
}

func partOneSample(t *testing.T) {
	t.Logf("\t\twith sample input")
	{
		dayOne := dayOne.NewDayOne([]byte(sampleInput))
		actual := dayOne.RunPartOne()

		assert(7, actual, t)
	}
}

func partOneActual(t *testing.T) {
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

func partTwoSample(t *testing.T) {
	t.Logf("\t\twith sample input")
	{
		dayOne := dayOne.NewDayOne([]byte(sampleInput))
		actual := dayOne.RunPartTwo()

		assert(5, actual, t)
	}
}

func partTwoActual(t *testing.T) {
	t.Logf("\t\twith actual input")
	{
		config := getConfig()
		client := adventclient.NewAdventClient(config.BaseUrl, config.SessionToken)

		input, err := client.GetInput(1)
		if err != nil {
			t.Fatalf("\t\t%s\tfailed to read input from AoC", failure)
		}

		dayOne := dayOne.NewDayOne([]byte(input))
		actual := dayOne.RunPartTwo()

		assert(1150, actual, t)
	}
}

func assert(expected int, actual int, t *testing.T) {
	if actual != expected {
		t.Fatalf("\t\t%s\texpected %v, got %v", failure, expected, actual)
	}

	t.Logf("\t\t%s\tshould return %v", success, expected)
}

func getConfig() TestConfig {
	content, err := os.ReadFile("../../test-config.json")
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
