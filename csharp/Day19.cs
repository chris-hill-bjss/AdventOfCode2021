internal sealed class Day19
{
    private readonly string[] _input;

    public Day19(string[] input)
    {
        _input = input;
    }

    public int SolvePartOne()
    {
        var scanners = ConvertInputToScannerCollection();

        var beacons = FindAbsoluteBeaconPositions(scanners, out _);

        return beacons.Count;
    }

    public int SolvePartTwo()
    {
        var scanners = ConvertInputToScannerCollection();

        var beacons = FindAbsoluteBeaconPositions(scanners, out var scannerPositions);

        return 
            scannerPositions
                .SelectMany(a => scannerPositions.Select(b => a.ManhattanDistance(b)))
                .Max();
    }

    private HashSet<Vector> FindAbsoluteBeaconPositions(
        HashSet<Vector>[] scanners, 
        out List<Vector> scannerPositions)
    {
        var absoluteBeacons = scanners[0];
        var unaligned = new Queue<HashSet<Vector>>(scanners[1..]);
        
        // we know exactly where scanner zero is
        scannerPositions = new() 
        { 
            new Vector(0,0,0)
        };

        while(unaligned.TryDequeue(out var beaconSet))
        {
            var matched = false;

            foreach (var rotation in MatrixRotation) 
            {
                var transformed = beaconSet.Select(rotation).ToArray();

                var scanner = 
                    transformed
                        .SelectMany(vectorA => absoluteBeacons.Select(vectorB => vectorA - vectorB))
                        .GroupBy(vector => vector, (k, grp) => (vector: k, count: grp.Count()))
                        .FirstOrDefault(t => t.count >= 12, (vector: Vector.Invalid, count: -1));
                
                if (scanner.vector == Vector.Invalid)
                    continue;

                matched = true;
                
                foreach(var vector in transformed)
                {
                    absoluteBeacons.Add(vector - scanner.vector);
                }

                scannerPositions.Add(scanner.vector);
                break;
            }

            // scanners aren't necessarily arranged in the order in the list
            // if we don't find anything this time come back to it after 
            // processing others.
            if (!matched) unaligned.Enqueue(beaconSet);
        }

        return absoluteBeacons;
    }

    static Func<Vector, Vector>[] MatrixRotation = new Func<Vector, Vector>[]
    {
        v => v,
        v => new(v.x, -v.y, -v.z),
        v => new(v.x, v.z, -v.y),
        v => new(v.x, -v.z, v.y),

        v => new(-v.x, v.y, -v.z),
        v => new(-v.x, -v.y, v.z),
        v => new(-v.x, v.z, v.y),
        v => new(-v.x, -v.z, -v.y),

        v => new(v.y, v.x, -v.z),
        v => new(v.y, -v.x, v.z),
        v => new(v.y, v.z, v.x),
        v => new(v.y, -v.z, -v.x),

        v => new(-v.y, v.x, v.z),
        v => new(-v.y, -v.x, -v.z),
        v => new(-v.y, v.z, -v.x),
        v => new(-v.y, -v.z, v.x),

        v => new(v.z, v.x, v.y),
        v => new(v.z, -v.x, -v.y),
        v => new(v.z, v.y, -v.x),
        v => new(v.z, -v.y, v.x),
        
        v => new(-v.z, v.x, -v.y),
        v => new(-v.z, -v.x, v.y),
        v => new(-v.z, v.y, v.x),
        v => new(-v.z, -v.y, -v.x),
    };

    private HashSet<Vector>[] ConvertInputToScannerCollection() =>
        _input
            .Select((readings, i) => 
                readings
                    .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .Select(r => {
                        var axes = r.Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(a => Convert.ToInt32(a))
                            .ToArray();

                        return new Vector(axes[0], axes[1], axes[2]);
                    })
                    .ToHashSet())
            .ToArray();

    record Vector(int x, int y, int z)
    {
        public static Vector Invalid = new Vector(-1, -1, -1);
        public static Vector operator -(Vector a, Vector b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
        public int ManhattanDistance(Vector other) => Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z);
    }

    public static string[] TestInput = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14"
    .Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
}