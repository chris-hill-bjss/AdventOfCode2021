package utility

func Map[F comparable, T comparable](from []F, mapper func(F) T) []T {
	to := make([]T, len(from))

	for i, v := range from {
		to[i] = mapper(v)
	}

	return to
}
