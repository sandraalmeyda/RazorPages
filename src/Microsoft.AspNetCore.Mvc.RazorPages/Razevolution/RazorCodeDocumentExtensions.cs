﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc.RazorPages.Razevolution
{
    public static class RazorCodeDocumentExtensions
    {
        public static RazorSyntaxTree GetSyntaxTree(this RazorCodeDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return (RazorSyntaxTree)document.Items[typeof(RazorSyntaxTree)];
        }

        public static void SetSyntaxTree(this RazorCodeDocument document, RazorSyntaxTree syntaxTree)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (syntaxTree == null)
            {
                throw new ArgumentNullException(nameof(syntaxTree));
            }

            document.Items[typeof(RazorSyntaxTree)] = syntaxTree;
        }

        public static void AddVirtualSyntaxTree(this RazorCodeDocument document, RazorSyntaxTree syntaxTree)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (syntaxTree == null)
            {
                throw new ArgumentNullException(nameof(syntaxTree));
            }

            GetVirtualSyntaxTrees(document).Add(syntaxTree);
        }

        public static IList<RazorSyntaxTree> GetVirtualSyntaxTrees(this RazorCodeDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var items = (IList<RazorSyntaxTree>)document.Items[typeof(List<RazorSyntaxTree>)];
            if (items == null)
            {
                items = new List<RazorSyntaxTree>();
                document.Items[typeof(List<RazorSyntaxTree>)] = items;
            }

            return items;
        }

        public static RazorChunkTree GetChunkTree(this RazorCodeDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return (RazorChunkTree)document.Items[typeof(RazorChunkTree)];
        }

        public static void SetChunkTree(this RazorCodeDocument document, RazorChunkTree chunkTree)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (chunkTree == null)
            {
                throw new ArgumentNullException(nameof(chunkTree));
            }

            document.Items[typeof(RazorChunkTree)] = chunkTree;
        }

        public static GeneratedCSharpDocument GetGeneratedCSharpDocument(this RazorCodeDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return (GeneratedCSharpDocument)document.Items[typeof(GeneratedCSharpDocument)];
        }

        public static void SetGeneratedCSharpDocument(this RazorCodeDocument document, GeneratedCSharpDocument code)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            document.Items[typeof(GeneratedCSharpDocument)] = code;
        }

        public static GeneratedClassInfo GetClassName(this RazorCodeDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return (GeneratedClassInfo)document.Items[typeof(GeneratedClassInfo)];
        }

        public static RazorCodeDocument WithClassName(this RazorCodeDocument document, string @namespace, string @class)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (@namespace == null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            if (@class == null)
            {
                throw new ArgumentNullException(nameof(@class));
            }

            var classInfo = new GeneratedClassInfo()
            {
                Class = @class,
                Namespace = @namespace,
            };

            document.Items[typeof(GeneratedClassInfo)] = classInfo;
            return document;
        }
    }
}
