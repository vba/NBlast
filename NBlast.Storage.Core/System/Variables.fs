namespace NBlast.Storage.Core.System

open System

module Variables = 
    let TempFolderPath = lazy(
        let result =
            [ Environment.GetEnvironmentVariable("TMPDIR");
              Environment.GetEnvironmentVariable("TEMP");
              Environment.GetEnvironmentVariable("TEMP_DIR") ]
                |> List.filter (fun x -> not (String.IsNullOrEmpty(x)))
                |> List.head
        if (String.IsNullOrEmpty(result)) 
            then raise(new InvalidOperationException("Impossible to go further with an empty temp path"))
        result
    )

    let UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)

    let RealOperatingSystem = 
        not(
            Environment
                .OSVersion
                .ToString()
                .ToUpperInvariant()
                .Contains("WINDOWS")
        )
