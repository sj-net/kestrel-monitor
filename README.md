# kestrel-monitor
A simple monitor for kestrel apps.


## Options
  `-f`, `--file`          Required. The file to run.

  `-c`, `--customArgs`    The custom arguments for the executable. ex: `' -e Development'`. Please start with space else all arguements and include single quotes are not getting supplied

  `--help`             Display this help screen.

 `--version`           Display version information.


Credits for [C# CommandLineParser](https://github.com/commandlineparser/commandline)

# Usage

1. [Download](https://github.com/sj-net/kestrel-monitor/releases/download/v1/kestrel-monitor.1.0.0.nupkg)
1. Install - `dotnet tool install --global --add-source <path to the dowbloaded nuget package> kestrel-monitor`
1. Usage `kestrel-monitor -f 'kesterl app path' -c ' -e Development -arg1 val1'`
1. Uninstall - `dotnet tool uninstall --global kestrel-monitor`
