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
reportunit "C:\my-folder\result.xml" reportunit "C:\output-folder\report.html"
```

### Snapshots

#### Folder
<img src='http://relevantcodes.com/Tools/ReportUnit/folder.png' />

#### File
<img src='http://relevantcodes.com/Tools/ReportUnit/file.png' />

### License

Copyright (c) 2015 Anshoo Arora (Relevant Codes)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
