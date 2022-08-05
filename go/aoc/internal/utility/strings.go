package utility

import (
	"log"
	"strconv"
)

func StringToInt(input string) int {
	value, err := strconv.Atoi(input)
	if err != nil {
		log.Fatal(err)
	}

	return value
}
