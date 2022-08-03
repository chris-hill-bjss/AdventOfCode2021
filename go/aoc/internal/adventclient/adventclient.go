package adventclient

import (
	"fmt"
	"io"
	"io/ioutil"
	"log"
	"net/http"
)

type AdventClient struct {
	baseUrl       string
	sessionCookie string
	client        *http.Client
}

func NewAdventClient(baseUrl string, sessionCookie string) *AdventClient {
	return &AdventClient{
		baseUrl:       baseUrl,
		sessionCookie: sessionCookie,
		client:        &http.Client{},
	}
}

func (ac *AdventClient) GetInput(day int) ([]byte, error) {
	request, err := createRequest(ac.baseUrl, day, ac.sessionCookie)
	if err != nil {
		return nil, err
	}

	response, err := ac.client.Do(request)
	if err != nil {
		return nil, err
	}

	defer func(body io.ReadCloser) {
		err = body.Close()
		if err != nil {
			log.Fatal("Failed to close response body reader")
		}
	}(response.Body)

	ensureSuccessStatusCode(response)

	bodyContent, err := ioutil.ReadAll(response.Body)
	if err != nil {
		return nil, err
	}

	return bodyContent, nil
}

func createRequest(baseUrl string, day int, sessionCookie string) (*http.Request, error) {
	request, err := http.NewRequest(
		http.MethodGet,
		fmt.Sprintf("%s/day/%v/input", baseUrl, day),
		nil)

	if err != nil {
		return nil, err
	}

	request.Header.Add("cookie", sessionCookie)
	return request, nil
}

func ensureSuccessStatusCode(response *http.Response) {
	if response.StatusCode != http.StatusOK {
		log.Panicf("unexpected status code %v from %v", response.StatusCode, response.Request.URL)
	}
}
