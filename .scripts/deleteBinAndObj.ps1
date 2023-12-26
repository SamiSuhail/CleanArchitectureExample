function Delete-BinObjDirectories {
    param (
        [string]$Path
    )

    $allSubDirectories = Get-ChildItem -LiteralPath $Path -Directory
    $subDirectoriesToCheckRecursively = Get-ChildItem -LiteralPath $Path -Directory | Where-Object { $_.Name -notmatch '^(bin|obj|\.)' }

    foreach ($directory in $subDirectoriesToCheckRecursively) {
        Delete-BinObjDirectories -Path $directory.FullName
    }

    # Remove bin and obj directories
    foreach ($directory in $allSubDirectories) {
        if ($directory.Name -eq 'bin' -or $directory.Name -eq 'obj') {
            Write-Host "Deleting $($directory.FullName)"
            Remove-Item -LiteralPath $directory.FullName -Recurse -Force
        }
    }
}

$parentDirectory = Get-Location | Split-Path -Parent

Delete-BinObjDirectories -Path $parentDirectory

Read-Host "Press Enter to continue..."