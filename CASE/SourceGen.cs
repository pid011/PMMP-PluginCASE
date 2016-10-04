using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CASE
{
    public static class SourceGen
    {
        public static string CreatePluginYML(ConstructureInfo info)
        {
            var sb = new StringBuilder();
            sb.AppendLine($@"name: {info.PluginName}");
            sb.AppendLine($@"main: {info.Author}\{info.PluginName}\{info.PluginName}");
            sb.AppendLine($@"author: {info.Author}");
            sb.AppendLine($"version: {info.PluginVersion}");
            sb.AppendLine($"api:");
            foreach (var item in info.API)
            {
                sb.AppendLine($"- {item}");
            }

            string source = sb.ToString();

            return source;
        }

        public static string CreateMainPHP(ConstructureInfo info)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"<?php");
            sb.AppendLine($@"namespace {info.Author}\{info.PluginName};");
            sb.AppendLine();
            sb.AppendLine(@"use pocketmine\plugin\PluginBase;");
            sb.AppendLine(@"use pocketmine\event\Listner;");
            sb.AppendLine();
            sb.Append($@"class {info.PluginName}");
            sb.AppendLine(@" extends PluginBase implements Listener{");
            sb.AppendLine("\tpublic function onEnable(){");
            sb.Append("\t\t");
            sb.AppendLine(@"$this->getServer()->getPluginManager()->registerEvents($this, $this);");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            sb.Append("?>");

            string source = sb.ToString();
            return source;
        }
    }
}
