' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis
Imports Roslyn.Test.Utilities

Namespace Microsoft.VisualStudio.LanguageServices.UnitTests.CodeModel.CSharp
    Public Class RootCodeModelTests
        Inherits AbstractRootCodeModelTests

#Region "CodeElements tests"

        <ConditionalFact(GetType(x86)), Trait(Traits.Feature, Traits.Features.CodeModel)>
        Public Sub CodeElements1()
            Dim code =
<code>
class Foo { }
</code>

            TestCodeElements(code,
                             "Foo",
                             "System",
                             "Microsoft")
        End Sub

#End Region

        <ConditionalFact(GetType(x86)), Trait(Traits.Feature, Traits.Features.CodeModel)>
        Public Sub TestExternalNamespaceChildren()
            Dim code =
<code>
class Foo { }
</code>

            TestRootCodeModel(code,
                Sub(rootCodeModel)
                    Dim systemNamespace = rootCodeModel.CodeElements.Find(Of EnvDTE.CodeNamespace)("System")
                    Assert.NotNull(systemNamespace)

                    Dim collectionsNamespace = systemNamespace.Members.Find(Of EnvDTE.CodeNamespace)("Collections")
                    Assert.NotNull(collectionsNamespace)

                    Dim genericNamespace = collectionsNamespace.Members.Find(Of EnvDTE.CodeNamespace)("Generic")
                    Assert.NotNull(genericNamespace)

                    Dim listClass = genericNamespace.Members.Find(Of EnvDTE.CodeClass)("List")
                    Assert.NotNull(listClass)
                End Sub)
        End Sub

#Region "CreateCodeTypeRef"

        <ConditionalFact(GetType(x86)), Trait(Traits.Feature, Traits.Features.CodeModel)>
        Public Sub CreateCodeTypeRef_Int32()
            TestCreateCodeTypeRef("System.Int32",
                                  New CodeTypeRefData With {
                                      .AsString = "int",
                                      .AsFullName = "System.Int32",
                                      .CodeTypeFullName = "System.Int32",
                                      .TypeKind = EnvDTE.vsCMTypeRef.vsCMTypeRefInt
                                  })
        End Sub

        <ConditionalFact(GetType(x86)), Trait(Traits.Feature, Traits.Features.CodeModel)>
        Public Sub CreateCodeTypeRef_System_Text_StringBuilder()
            TestCreateCodeTypeRef("System.Text.StringBuilder",
                                  New CodeTypeRefData With {
                                      .AsString = "System.Text.StringBuilder",
                                      .AsFullName = "System.Text.StringBuilder",
                                      .CodeTypeFullName = "System.Text.StringBuilder",
                                      .TypeKind = EnvDTE.vsCMTypeRef.vsCMTypeRefCodeType
                                  })
        End Sub

        <ConditionalFact(GetType(x86)), Trait(Traits.Feature, Traits.Features.CodeModel)>
        Public Sub CreateCodeTypeRef_NullableInteger()
            TestCreateCodeTypeRef("int?",
                                  New CodeTypeRefData With {
                                      .AsString = "int?",
                                      .AsFullName = "System.Nullable<System.Int32>",
                                      .CodeTypeFullName = "System.Int32?",
                                      .TypeKind = EnvDTE.vsCMTypeRef.vsCMTypeRefCodeType
                                  })
        End Sub

        <ConditionalFact(GetType(x86)), Trait(Traits.Feature, Traits.Features.CodeModel)>
        Public Sub CreateCodeTypeRef_ListOfInt()
            TestCreateCodeTypeRef("System.Collections.Generic.List<int>",
                                  New CodeTypeRefData With {
                                      .AsString = "System.Collections.Generic.List<int>",
                                      .AsFullName = "System.Collections.Generic.List<System.Int32>",
                                      .CodeTypeFullName = "System.Collections.Generic.List<System.Int32>",
                                      .TypeKind = EnvDTE.vsCMTypeRef.vsCMTypeRefCodeType
                                  })
        End Sub

#End Region

        Protected Overrides ReadOnly Property LanguageName As String
            Get
                Return LanguageNames.CSharp
            End Get
        End Property
    End Class
End Namespace
