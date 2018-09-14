using System;
using System.IO;

using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class ShellModule : Module
    {
        public ShellModule(Context parent) : base(parent, "shell")
        {
        }

        [Builtin("cls", InArity = 0)]
        private static void ClearScreen(Context context, Stack stack)
        {
            Console.Clear();
        }

        [Builtin("dir", InArity = 0)]
        private static void ListDirectory(Context context, Stack stack)
        {
            var directory = new DirectoryInfo(Environment.CurrentDirectory);

            var result = Sequence.From(directory.EnumerateFileSystemInfos());

            stack.Push(result);
        }

        [Builtin("cwd", InArity = 0)]
        private static void GetCurrentDirectory(Context context, Stack stack)
        {
            var result = new DirectoryInfo(Environment.CurrentDirectory);

            stack.Push(result);
        }
    }
}
