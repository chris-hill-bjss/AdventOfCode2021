package inputparser

import (
	"strings"
)

func ToStringArray(input []byte) []string {
	return strings.Split(strings.TrimSpace(string(input)), "\n")
}
