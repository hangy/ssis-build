if (args.Length < 1)
{
    return -1;
}

if ("build".Equals(args[0], StringComparison.OrdinalIgnoreCase))
{
    SsisBuild.Program.Main(args[1..]);
    return 0;
}
else if ("deploy".Equals(args[0], StringComparison.OrdinalIgnoreCase))
{
    SsisDeploy.Program.Main(args[1..]);
    return 0;
}

return -1;