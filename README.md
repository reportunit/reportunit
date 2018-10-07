[![Build Status][appveyor_status]][appveyor_link]
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bhttps%3A%2F%2Fgithub.com%2Fekirmayer%2Freportunit.svg?type=shield)](https://app.fossa.io/projects/git%2Bhttps%3A%2F%2Fgithub.com%2Fekirmayer%2Freportunit?ref=badge_shield)

[appveyor_status]: https://ci.appveyor.com/api/projects/status/q4cmp3mw32e31oy7?branch=master&svg=true
[appveyor_link]: https://ci.appveyor.com/project/Artum/reportunit


# ReportUnit
ReportUnit is a report generator for the test-runner family. It uses stock reports from NUnit, MSTest and Gallio and converts them into attractive HTML reports with dashboards.

Samples:

<ul>
<li><a href='http://relevantcodes.com/Tools/ReportUnit/Index.html'>Folder - Executive Summary</a></li>
<li><a href='http://relevantcodes.com/Tools/ReportUnit/NUnit-TestResult.html'>Folder - Single File</a></li>
<li><a href='http://relevantcodes.com/Tools/ReportUnit/NUnit-TestResult-standalone.html'>File Level</a></li>
</ul>

### Download Exe

Download using <a href='http://relevantcodes.com/reportunit'>this</a> link.

### Usage: Building Folder-Level Summary
You can either generate a report for all files in a folder, or simply convert a given file. Both methods are shown below.

To build a summary for all supported test runner files, simply open cmd.exe and point to the folder where the XML files are stored:

```
reportunit [input-folder-path]
reportunit [input-folder-path] [output-folder-path]
```

```
// all files will be created in the current folder
reportunit .

// all files will be created in the my-folder
reportunit "c:\my-folder"

// files will be created in output-folder
reportunit "c:\my-folder" "c:\output-folder
```

### Usage: Building TestSuite-Level Summary

To build report from any NUnit TestResult XML file, point to the input file and also specify the name of the output file:

```
reportunit [input-file]
reportunit [input-file] [output-file]
```

```
reportunit "C:\my-folder\result.xml"
reportunit "C:\my-folder\result.xml" "C:\output-folder\report.html"
```

### Snapshots

#### Folder
<img src='http://relevantcodes.com/Tools/ReportUnit/folder.png' />

#### File
<img src='http://relevantcodes.com/Tools/ReportUnit/file.png' />



## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bhttps%3A%2F%2Fgithub.com%2Fekirmayer%2Freportunit.svg?type=large)](https://app.fossa.io/projects/git%2Bhttps%3A%2F%2Fgithub.com%2Fekirmayer%2Freportunit?ref=badge_large)
