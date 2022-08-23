using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using MySql.Data.MySqlClient.Authentication;
using Terraria.UI;

namespace TestPlugin
{
    [ApiVersion(2, 1)]//api版本
    public class TestPlugin : TerrariaPlugin
    {
        /// 插件作者
        public override string Author => "奇威复反";
        /// 插件说明
        public override string Description => "指定位置召唤NPC";
        /// 插件名字
        public override string Name => "指定位置召唤NPC";
        /// 插件版本
        public override Version Version => new Version(1, 0, 0, 0);
        /// 插件处理
        public TestPlugin(Main game) : base(game)
        {
        }
        /// 插件启动时，用于初始化各种狗子
        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);//钩住游戏初始化时
        }
        /// 插件关闭时
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Deregister hooks here
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);//销毁游戏初始化狗子

            }
            base.Dispose(disposing);
        }

        private void OnInitialize(EventArgs args)//游戏初始化的狗子
        {
            //第一个是权限，第二个是子程序，第三个是名字

            Commands.ChatCommands.Add(
                new Command("sm2", _指定位置召唤NPC, "sm2")
                {
                });
        }
        private void _指定位置召唤NPC(CommandArgs args)
        {
            try
            {
                if (args.Parameters.Count == 0)
                {
                    args.Player.SendSuccessMessage(args.Player.Name + "正确指令：/sm2 NpcID X Y 数量");
                    return;
                }
                string n = args.Parameters[0];
                string x = args.Parameters[1];
                string y = args.Parameters[2];
                int n2 = Convert.ToInt32(n);
                int x2 = 16 * Convert.ToInt32(x);
                int y2 = 16 * Convert.ToInt32(y);
                try
                {
                    string s = args.Parameters[3];
                    int s2 = Convert.ToInt32(s);
                    if (s2 > 0)
                    {
                        args.Player.SendSuccessMessage("已在" + x + "," + y + "召唤" + s + "个" + n2);
                        while (s2 > 0)
                        {
                            int index = NPC.NewNPC(null, x2, y2, n2);
                            NetMessage.SendData((byte)PacketTypes.NpcUpdate, -1, -1, null, index);
                            s2--;
                            return;
                        }
                    }
                }
                catch
                {
                    int index = NPC.NewNPC(null, x2, y2, 1);
                    NetMessage.SendData((byte)PacketTypes.NpcUpdate, -1, -1, null, index);
                    args.Player.SendSuccessMessage("已在" + x + "," + y + "召唤1个" + n2);
                    return;
                }
            }
            catch
            {
                args.Player.SendSuccessMessage("正确指令：/sm2 NpcID X Y 数量");
                return;
            }

        }
    }
}
