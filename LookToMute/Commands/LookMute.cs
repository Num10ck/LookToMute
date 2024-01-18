using CommandSystem;
using Exiled.API.Features;
using System;
using UnityEngine;
using MapEditorReborn;
using Exiled.API.Features.Toys;

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

            Debug.DrawRay(playerSender.Position, playerSender.CameraTransform.forward + new Vector3(0, 0, 0.5f), Color.red, 10);
            Ray ray = new Ray(playerSender.CameraTransform.position, playerSender.CameraTransform.forward);

            Arguments arg = (Arguments)Enum.Parse(typeof(Arguments), arguments.FirstElement(), true);

            return Action(ray, arg, out response);
        }

        bool Action(Ray ray, Arguments arg, out string response)
        {
            
            string responseFunc(Player ply)
            {
                string state = ply.IsMuted ? "muted" : "unmuted";
                return $"Player {ply.Nickname} is " + state;
            }

            if (Physics.SphereCast(ray, config.Radius, maxDistance: config.MaxDistance, hitInfo: out RaycastHit hit))
            {
                if (Player.TryGet(hit.collider, out Player ply))
                {
                    if (arg == Arguments.Mute)
                    {
                        ply.IsMuted = true;
                        response = responseFunc(ply);
                        return true;
                    }
                    else if (arg == Arguments.Unmute)
                    {
                        ply.IsMuted = false;
                        response = responseFunc(ply);
                        return true;
                    }
                    else
                    {
                        if (ply.IsMuted)
                        {
                            ply.IsMuted = false;
                        }
                        else
                        {
                            ply.IsMuted = true;
                        }
                        response = responseFunc(ply);
                        return true;
                    }
                }
            }
            response = "No players at view";
            return false;
        }

        enum Arguments
        {
            Mute,
            Unmute,
            Switch
        }
    }
}
