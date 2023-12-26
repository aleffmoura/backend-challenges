namespace Challenge.CLI.Tests.Program;

using FluentAssertions;
using System.Diagnostics;

[TestFixture]
public class MyProgramTests
{
    private ProcessStartInfo _processStartInfo;

    [SetUp]
    public void Setup()
    {
        _processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "Challenge.CLI.dll",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
    }

    [Test]
    public void MyProgramTests_ExecuteExeWithInput_Case1()
    {
        //arrange
        var caminhoArquivoEntrada = "Program/files/case1.txt";

        var inputContent = File.ReadAllText(caminhoArquivoEntrada);

        //action
        using var process = new Process { StartInfo = _processStartInfo };
        process.Start();

        process.StandardInput.Write(inputContent);
        process.StandardInput.Flush();
        process.StandardInput.Close();
        process.WaitForExit();

        //assetions
        string output = process.StandardOutput.ReadToEnd().Replace("\r\n", "").Trim();
        output.Should().Be("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]");
    }

    [Test]
    public void MyProgramTests_ExecuteExeWithInput_Case2()
    {
        //arrange
        var caminhoArquivoEntrada = "Program/files/case2.txt";

        var inputContent = File.ReadAllText(caminhoArquivoEntrada);


        //action
        using var process = new Process { StartInfo = _processStartInfo };
        process.Start();

        process.StandardInput.Write(inputContent);
        process.StandardInput.Flush();
        process.StandardInput.Close();
        process.WaitForExit();

        //assetions
        string output = process.StandardOutput.ReadToEnd().Replace("\r\n", "").Trim();
        output.Should().Be("[{\"tax\":0.00},{\"tax\":10000.00},{\"tax\":0.00}]");
    }

    [Test]
    public void MyProgramTests_ExecuteExeWithInput_Case1plus2()
    {
        //arrange
        var caminhoArquivoEntrada = "Program/files/case1plus2.txt";

        var inputContent = File.ReadAllText(caminhoArquivoEntrada);

        //action
        using var process = new Process { StartInfo = _processStartInfo };
        process.Start();

        process.StandardInput.Write(inputContent);
        process.StandardInput.Flush();
        process.StandardInput.Close();
        process.WaitForExit();

        //assetions
        string output = process.StandardOutput.ReadToEnd().Replace("\r\n", "\n").Trim();
        output.Should().Be("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]\n[{\"tax\":0.00},{\"tax\":10000.00},{\"tax\":0.00}]");
    }

    [Test]
    public void MyProgramTests_ExecuteExeWithInput_Case3()
    {
        //arrange
        var caminhoArquivoEntrada = "Program/files/case3.txt";

        var inputContent = File.ReadAllText(caminhoArquivoEntrada);

        //action
        using var process = new Process { StartInfo = _processStartInfo };
        process.Start();

        process.StandardInput.Write(inputContent);
        process.StandardInput.Flush();
        process.StandardInput.Close();
        process.WaitForExit();

        //assetions
        string output = process.StandardOutput.ReadToEnd().Replace("\r\n", "").Trim();
        output.Should().Be("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":1000.00}]");
    }
    [Test]
    public void MyProgramTests_ExecuteExeWithInput_Case4()
    {
        //arrange
        var caminhoArquivoEntrada = "Program/files/case4.txt";

        var inputContent = File.ReadAllText(caminhoArquivoEntrada);

        //action
        using var process = new Process { StartInfo = _processStartInfo };
        process.Start();

        process.StandardInput.Write(inputContent);
        process.StandardInput.Flush();
        process.StandardInput.Close();
        process.WaitForExit();

        //assetions
        string output = process.StandardOutput.ReadToEnd().Replace("\r\n", "").Trim();
        output.Should().Be("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]");
    }
    [Test]
    public void MyProgramTests_ExecuteExeWithInput_Case5()
    {
        //arrange
        var caminhoArquivoEntrada = "Program/files/case5.txt";

        var inputContent = File.ReadAllText(caminhoArquivoEntrada);

        //action
        using var process = new Process { StartInfo = _processStartInfo };
        process.Start();

        process.StandardInput.Write(inputContent);
        process.StandardInput.Flush();
        process.StandardInput.Close();
        process.WaitForExit();

        //assetions
        string output = process.StandardOutput.ReadToEnd().Replace("\r\n", "").Trim();
        output.Should().Be("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":10000.00}]");
    }
    [Test]
    public void MyProgramTests_ExecuteExeWithInput_Case6()
    {
        //arrange
        var caminhoArquivoEntrada = "Program/files/case6.txt";

        var inputContent = File.ReadAllText(caminhoArquivoEntrada);

        //action
        using var process = new Process { StartInfo = _processStartInfo };
        process.Start();

        process.StandardInput.Write(inputContent);
        process.StandardInput.Flush();
        process.StandardInput.Close();
        process.WaitForExit();

        //assetions
        string output = process.StandardOutput.ReadToEnd().Replace("\r\n", "").Trim();
        output.Should().Be("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3000.00}]");
    }
    [Test]
    public void MyProgramTests_ExecuteExeWithInput_Case7()
    {
        //arrange
        var caminhoArquivoEntrada = "Program/files/case7.txt";

        var inputContent = File.ReadAllText(caminhoArquivoEntrada);

        //action
        using var process = new Process { StartInfo = _processStartInfo };
        process.Start();

        process.StandardInput.Write(inputContent);
        process.StandardInput.Flush();
        process.StandardInput.Close();
        process.WaitForExit();

        //assetions
        string output = process.StandardOutput.ReadToEnd().Replace("\r\n", "").Trim();
        output.Should().Be("[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3000.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3700.00},{\"tax\":0.00}]");
    }
    [Test]
    public void MyProgramTests_ExecuteExeWithInput_Case8()
    {
        //arrange
        var caminhoArquivoEntrada = "Program/files/case8.txt";
        var inputContent = File.ReadAllText(caminhoArquivoEntrada);

        //action
        using var process = new Process { StartInfo = _processStartInfo };
        process.Start();

        process.StandardInput.Write(inputContent);
        process.StandardInput.Flush();
        process.StandardInput.Close();
        process.WaitForExit();

        //assetions
        string output = process.StandardOutput.ReadToEnd().Replace("\r\n", "").Trim();
        output.Should().Be("[{\"tax\":0.00},{\"tax\":80000.00},{\"tax\":0.00},{\"tax\":60000.00}]");
    }
}