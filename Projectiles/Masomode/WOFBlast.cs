using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class WOFBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divine Blast");
            Main.projFrames[projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 80;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.scale = 1.5f;
            projectile.alpha = 0;
        }

        public override void AI()
        {
            int ai1 = (int)projectile.ai[1];

            if (projectile.position.HasNaNs() && !(ai1 > -1 && ai1 < Main.maxNPCs && Main.npc[ai1].active && Main.npc[ai1].type == NPCID.WallofFleshEye))
            {
                projectile.Kill();
                return;
            }
            
            Vector2 offset;
            if (projectile.ai[0] == 0f)
            {
                projectile.rotation = Main.npc[ai1].rotation + (float)Math.PI;
                offset = new Vector2(Main.npc[ai1].width - 36, 6).RotatedBy(projectile.rotation);
            }
            else
            {
                projectile.rotation = Main.npc[ai1].rotation;
                offset = new Vector2(Main.npc[ai1].width - 36, -6).RotatedBy(projectile.rotation);
            }
            projectile.Center = Main.npc[ai1].Center + offset;
            
            if (++projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame--;
                    projectile.Kill();
                }
            }
            
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1f;
                Main.PlaySound(SoundID.Item88, projectile.Center);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            SpriteEffects effects = projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, effects, 0f);
            return false;
        }
    }
}

