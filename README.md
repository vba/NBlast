[![Build Status](https://travis-ci.org/vba/NBlast.svg?branch=master)](https://travis-ci.org/vba/NBlast)
NBlast
======

NBlast is a lightweight log management tool. NBlast comes with two parts, REST-based log register and web iterface for searching and visualization of yours logs. NBlast's REST API is entirely based on .NET stack and depends only on it. 

It's totally open source. NBlast uses Apache 2.0 licence that gives you a lot of liberty to use it whatever you want.

NBlast is quite simple, easy in configuration, deployment and usage. It's based on such well-recommended project as Owin, WebApi and Lucene.NET.

NBlast was widely tested with NLog but it can easily work with large variety of log emitters.

Requirements
-------
##### Runtime
* .NET Framework v4.5 or above

##### Build
* F# v3.1 or above
* npm v1.4.21 or above
  * gulp v3.8.11 or above
  * bower v1.3.12 or above
  * coffee v1.8.0 or above

Build
-------
##### Server side
```bash
git clone <NBLAST.git>
cd NBlast
build.cmd # or build.sh
```

##### Client side
```bash
git clone <NBLAST.git>
cd NBlast/NBlast.Client
npm install
bower install
gulp package
 mv .\out\nblast.client.zip <DESTINATION>
```


Configuration
--------
##### NBlast.Api
*TODO*

##### NBlast.Client
*TODO*

Install
--------
##### NBlast.Api
NBlast.Api is built on [Topshelf](http://topshelf-project.com), so it can be deployed as windows service with ease. Topshelf is a cross-platform solution you can read more about its configuration in official [documentation](http://docs.topshelf-project.com/en/latest/).

First you need to review the configuration file *NBlast.Api.exe.config* and specify an appropriated index location.
```xml
<add key="NBlast.directoryPath" value="C:/Data/Logs/NBlast/index"/>
```

Then you need to authorize NBlast.Api to use its ports on localhost, for do that, you'll need to open a command prompt with administrator privileges and type following instruction:
```bash
netsh http add urlacl url=http://+:<PORT>/ user=<domain\username>
``` 
Where <domain\username> is your user account and <PORT> desired port. 

In the next step you need to install NBLast.Api as service, you could do so with following instruction:
```bash
.\NBlast.Api.exe service install -username:<domain\username> -password:**** --autostart
```
Where <domain\username> is an account to run your windows service with.

You can get more available options with:
```bash
.\NBlast.Api.exe help
```

If everything is correct, your service will start immediately. 

##### NBlast.Client
*TODO*

Usage
--------
##### Use with NLog target
*TODO*

##### Populate NBlast with event logs
```powershell
$restUri="http://<ADDRESS>:<PORT>/api/indexer/index"
$computer = Get-WmiObject -Class Win32_ComputerSystem 

Get-EventLog -List | %{ 
    $loggerStart = $_.LogDisplayName 
    Get-EventLog -LogName $_.Log -ErrorAction SilentlyContinue  | %{
        $level = switch ($_.EntryType.ToString().ToUpperInvariant()) {
            "WARNING" {"warn"}
            "ERROR" {"error"}
            "FAILUREAUDIT" {"error"}
            "FATAL" {"fatal"}
            "DEBUG" {"debug"}
            "INFORMATION" {"info"}
            default {"trace"}
        }
        $logModel = @{
            'sender' = $computer.Name
            'level' = $level
            'message' = $_.Message
            'logger' = $loggerStart + ' / ' + $_.Source
            'createdAt' = $_.TimeGenerated.ToString("s", [System.Globalization.CultureInfo]::InvariantCulture )
        } 
        $logModel | ConvertTo-Json
    } | %{
        Invoke-RestMethod -Uri $restUri -Method Post -ContentType 'application/json' -Body $_
    }
}
```

Licence
------
See details [here](https://raw.githubusercontent.com/vba/NBlast/master/LICENSE).
