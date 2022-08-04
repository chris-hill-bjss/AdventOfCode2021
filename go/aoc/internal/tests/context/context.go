package context

import (
	"aoc/internal/adventclient"
	"aoc/internal/tests/config"
)

type TestContext struct {
	*adventclient.AdventClient
}

func NewContext() *TestContext {
	config := config.LoadConfig()

	client := adventclient.NewAdventClient(config.BaseUrl, config.SessionToken)

	return &TestContext{client}
}
