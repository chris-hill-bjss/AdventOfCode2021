package assertions

import "testing"

const success = "\u2713"
const failure = "\u2717"

type Asserter struct {
	t *testing.T
}

func Assert(t *testing.T) *Asserter {
	return &Asserter{t}
}

func (a *Asserter) Equals(expected interface{}, actual interface{}) {
	if actual != expected {
		a.t.Errorf("\t%s\texpected %v, got %v", failure, expected, actual)
		return
	}

	a.t.Logf("\t%s\tshould return %v", success, expected)
}

func (a *Asserter) Fail(message string) {
	a.t.Errorf("\t%s\t%s", failure, message)
}
