using CommandSystem;
using Exiled.API.Features;
using System;
using UnityEngine;

namespace LookToMute.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class LookMute : ICommand
    {
        public string Command => "lookmute";
        public string[] Aliases => new string[] { "lm" };
        public string Description => "Mutes player you're looking at";

        private Config config = new Config();
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);
            

            if (!sender.CheckPermission(PlayerPermissions.KickingAndShortTermBanning))
            {
                response = "You doesn't have permissions to use this command";
                return false;
            }

            Ray ray = new Ray(playerSender.Position, playerSender.CameraTransform.forward);

            switch (arguments.At(0).ToLower())
            {
                case "mute":
                    {
                        if (Physics.Raycast(ray, maxDistance: config.MaxDistance, hitInfo: out RaycastHit hit))
                        {
                            if (Player.TryGet(hit.collider, out Player ply))
                            {
                                ply.Mute();

                                response = $"Player {ply.Nickname} muted.";
                                return true;
                            }
                        }
                        response = "No players at view.";
                        return false;
                    }
                case "unmute":
                    {
                        if (Physics.Raycast(ray, maxDistance: config.MaxDistance, hitInfo: out RaycastHit hit))
                        {
                            if (Player.TryGet(hit.collider, out Player ply))
                            {
                                ply.UnMute();

                                response = $"Player {ply.Nickname} unmuted.";
                                return true;
                            }
                        }
                        response = "No players at view.";
                        return false;
                    }
                case "switch":
                    {
                        if (Physics.Raycast(ray, maxDistance: config.MaxDistance, hitInfo: out RaycastHit hit))
                        {
                            if (Player.TryGet(hit.collider, out Player ply))
                            {
                                if (ply.IsMuted)
                                {
                                    ply.UnMute();

                                    response = $"Player {ply.Nickname} unmuted.";
                                    return true;
                                }
                                ply.Mute();

                                response = $"Player {ply.Nickname} muted.";
                                return true;
                            }
                        }                 
                        response = "No players at view.";
                        return false;
                    }
            }
            response = "Specify valid subcommand. Avaliable subcommands: mute, unmute, switch";
            return false;
        }
    }
}
