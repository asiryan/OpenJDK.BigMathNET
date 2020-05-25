Advanced Java big math functions implementation in C# using [**IKVM.NET**](http://www.ikvm.net/)

## What is IKVM.NET?
**IKVM.NET** is an implementation of Java for [**Mono**](https://www.mono-project.com/) and the [**.NET Framework**](https://dotnet.microsoft.com/). It includes the following components:
* A Java Virtual Machine implemented in .NET  
* A .NET implementation of the Java class libraries  
* Tools that enable Java and .NET interoperability  

Read more about what you can do with [**IKVM.NET**](http://www.ikvm.net/uses.html).

## What is OpenJDK.BigMathNET?
**OpenJDK.BigMathNET** is a C# port of the Java [**library**](https://github.com/eobermuhlner/big-math) for advanced mathematical functions with arbitrary precision depended on **IKVM.NET**. It implements the following components:
* BigDecimalMath  
* BigComplexMath  
* BigRational  
* BigComplex  

and extends **java.math** functionality in **IKVM.NET**.  
To use it in your own project download the library from release [**folder**](https://github.com/asiryan/OpenJDK.BigMathNET/tree/master/release) or use [**nuget**](https://www.nuget.org/packages/OpenJDK.BigMathNET/) package manager.  
```c#
using java.math;
```
Usage example with calculating square root of 2
```c#
MathContext context = new MathContext(64);
BigDecimal a = new BigDecimal("2.0");
BigDecimal b = a.sqrt(context);
Console.WriteLine(b);
```
will produce the following output to the console:
```
1.414213562373095048801688724209698078569671875376948073176679738
```

## License
**GNU GPL v3.0**
