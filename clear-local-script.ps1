Write-Host "[MCIO] Limpando pastas bin e obj"

Write-Host "    ./src/Core/bin"
if (Test-Path "./src/Core/bin" -PathType Container)
{
    Remove-Item -Path "./src/Core/bin" -Recurse -Force
}

Write-Host "    ./src/Core/obj"
if (Test-Path "./src/Core/obj" -PathType Container)
{
    Remove-Item -Path "./src/Core/obj" -Recurse -Force
}

Write-Host "    ./tst/UnitTests/bin"
if (Test-Path "./tst/UnitTests/bin" -PathType Container)
{
    Remove-Item -Path "./tst/UnitTests/bin" -Recurse -Force
}

Write-Host "    ./tst/UnitTests/obj"
if (Test-Path "./tst/UnitTests/obj" -PathType Container)
{
    Remove-Item -Path "./tst/UnitTests/obj" -Recurse -Force
}

Write-Host "    ./benchs/Benchmarks/bin"
if (Test-Path "./benchs/Benchmarks/bin" -PathType Container)
{
    Remove-Item -Path "./benchs/Benchmarks/bin" -Recurse -Force
}

Write-Host "    ./benchs/Benchmarks/obj"
if (Test-Path "./benchs/Benchmarks/obj" -PathType Container)
{
    Remove-Item -Path "./benchs/Benchmarks/obj" -Recurse -Force
}

Write-Host "    ./samples/SampleApi/bin"
if (Test-Path "./samples/SampleApi/bin" -PathType Container)
{
    Remove-Item -Path "./samples/SampleApi/bin" -Recurse -Force
}

Write-Host "    ./samples/SampleApi/obj"
if (Test-Path "./samples/SampleApi/obj" -PathType Container)
{
    Remove-Item -Path "./samples/SampleApi/obj" -Recurse -Force
}

Write-Host "[MCIO] Limpando arquivos gerados nos teste"

Write-Host "    ./tst/UnitTests/.stryker-output"
if (Test-Path "./tst/UnitTests/.stryker-output" -PathType Container)
{
    Remove-Item -Path "./tst/UnitTests/.stryker-output" -Recurse -Force
}

Write-Host "    ./tst/UnitTests/Coverage"
if (Test-Path "./tst/UnitTests/Coverage" -PathType Container)
{
    Remove-Item -Path "./tst/UnitTests/Coverage" -Recurse -Force
}

Write-Host "[MCIO] Limpando pastas das ferramentas de CLI"

Write-Host "    ./.reportgenerator"
if (Test-Path "./.reportgenerator" -PathType Container)
{
    Remove-Item -Path "./.reportgenerator" -Recurse -Force
}
Write-Host "    ./.stryker"
if (Test-Path "./.stryker" -PathType Container)
{
    Remove-Item -Path "./.stryker" -Recurse -Force
}