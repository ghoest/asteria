﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Dynamic;
using Microsoft.Xna.Framework.Graphics;

namespace HD.Asteria
{
    public static class Enemies
    {
        public static void RegisterEnemies()
        {
            EnemyBase.AddItem(new EnemyType {
                Id = 1,
                Name = "Crawler",
                SpriteWidth = 57,
                SpriteHeight = 38,
                SpriteFramesPerRow = 5,
                MaxHealth = 11,
                Speed = 100,
                CollisionDamage = 11,
                CollisionKnockback = 500,
                IsAutoSpawn = true,
                AvoidsCliffs = true,
                BloodColor = Color.FromNonPremultiplied(189, 240, 64, 255),
                OnSpawn = (enemy, tier) => {
                    enemy.IsFacingLeft = Utility.Flip();
                    enemy.SetAnimation(Animation.Move);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                    enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position, Type = ParticleEffect.Splash, Color = Color.FromNonPremultiplied(204, 109, 27, 255), Value = 2 });
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked) {
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 50 % 10;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm);
                    else
                        enemy.Drop(ItemId.Energy);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.Thyratron, Utility.Next(enemy.Tier) + 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 33,
                Name = "Bulletproof Crawler",
                SoundName = "Crawler",
                SpriteWidth = 57,
                SpriteHeight = 38,
                SpriteFramesPerRow = 5,
                MaxHealth = 10000,
                Defense = Int32.MaxValue,
                Speed = 100,
                CollisionDamage = 11,
                CollisionKnockback = 500,
                IsAutoSpawn = true,
                MinSpawnTier = 5,
                SpawnRateLimitInSeconds = 180,
                AvoidsCliffs = true,
                BloodColor = Color.FromNonPremultiplied(189, 240, 64, 255),
                OnSpawn = (enemy, tier) => {
                    enemy.IsFacingLeft = Utility.Flip();
                    enemy.SetAnimation(Animation.Move);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked) {
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 50 % 10;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    //if (Utility.Roll128())
                    //    enemy.Drop(ItemId.Blaster9);
                    //else if (Utility.Roll128())
                    //    enemy.Drop(ItemId.Disruptor9);
                    //else if (Utility.Roll128())
                    //    enemy.Drop(ItemId.GrenadeLauncher9);
                    //else if (Utility.Roll128())
                    //    enemy.Drop(ItemId.LaserRifle9);
                    //else if (Utility.Roll128())
                    //    enemy.Drop(ItemId.Homing9);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 2,
                Name = "Drake",
                Description = "Flies at players",
                SpriteWidth = 32,
                SpriteHeight = 32,
                MaxHealth = 10,
                Speed = 200,
                CollisionDamage = 6,
                CollisionKnockback = 300,
                IsFlying = true,
                IsAutoSpawn = true,
                MinSpawnTier = 2,
                OnSpawn = (enemy, tier) => {
                    enemy.SetRandomAngle();
                },
                OnLongThink = (enemy) => {
                    if (enemy.Target != null && enemy.IsTargetCloserThan(800)) {
                        enemy.SetAngleToTarget();
                    } else {
                        if (Utility.Roll16() && enemy.Target == null || enemy.IsBlocked)
                            enemy.SetRandomAngle();
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm);
                    else
                        enemy.Drop(ItemId.Energy);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.FieldEffectTransistor, Utility.Next(enemy.Tier) + 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 31,
                Name = "Flame Drake",
                SoundName = "Drake",
                Description = "Flies and shoots at players",
                SpriteWidth = 32,
                SpriteHeight = 32,
                MaxHealth = 10,
                Speed = 200,
                CollisionDamage = 4,
                CollisionKnockback = 600,
                IsFlying = true,
                MinSpawnTier = 3,
                IsAutoSpawn = true,
                SpawnRateLimitInSeconds = 180,
                OnSpawn = (enemy, tier) => {
                    enemy.SetRandomAngle();
                    enemy.IsImmuneToEnvironment = true;
                },
                OnLongThink = (enemy) => {
                    ///Hover over player
                    if (!enemy.IsTargetCloserThan(300))
                        enemy.SetAngleToTarget();

                    if (enemy.IsTargetCloserThan(150))
                        enemy.SetAngleAwayFromTarget();

                    if (enemy.Target != null && enemy.CooldownCheck(1250) && enemy.IsTargetCloserThan(600))
                        enemy.ShootAtTarget(ProjectileId.MiniFlame_2);

                    if (Utility.Roll16() && enemy.Target == null)
                        enemy.SetRandomAngle();

                    if (enemy.IsBlocked)
                        enemy.SetRandomAngle();
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.GunnDiode, Utility.Next(enemy.Tier) + 1); ;
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 3,
                Name = "Turret",
                SpriteWidth = 32,
                SpriteHeight = 30,
                MaxHealth = 45,
                Speed = 0,
                CollisionDamage = 10,
                CollisionKnockback = 500,
                IsAutoSpawn = true,
                BloodColor = Color.Black,
                MinSpawnTier = 3,
                AlternateTextureName = "Turret Gun",
                ShootingOrigin = new Vector2(0, -13),
                ShootingOffset = 20,
                CoolDown = 1800,
                OnDrawTop = (enemy, spriteBatch, position, frame) => {
                    spriteBatch.Draw(enemy.Type.AlternateTexture, position + new Vector2(0, -13), null, Color.White, enemy.GetShootingAngle(), new Vector2(enemy.Type.AlternateTexture.Width / 2, enemy.Type.AlternateTexture.Height / 2), 1, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                },
                OnSpawn = (enemy, tier) => {
                    enemy.Bag.IsDeactivated = false;
                    enemy.Bag.DeactivatedSoundPlayed = false;
                    enemy.Bag.IsActivated = false;
                    enemy.Bag.ActivatedSoundPlayed = false;
                },
                OnLongThink = (enemy) => {
                    if (enemy.Target != null) {
                        enemy.ShootAtTarget(ProjectileId.LaserRifle);
                        enemy.Bag.IsDeactivated = false;
                        enemy.Bag.IsActivated = true;
                        enemy.Bag.DeactivatedSoundPlayed = false;
                    }
                    if (enemy.Bag.IsActivated && !enemy.Bag.ActivatedSoundPlayed) {
                        enemy.PlaySound(Sound.TurretActivate);
                        enemy.Bag.IsActivated = false;
                        enemy.Bag.ActivatedSoundPlayed = true;
                    }

                    if (enemy.Target != null && (!enemy.IsTargetCloserThan(510) && enemy.IsTargetCloserThan(700))) {
                        enemy.Bag.IsDeactivated = true;
                        enemy.Bag.IsActivated = false;
                        enemy.Bag.ActivatedSoundPlayed = false;
                    }
                    if (enemy.Bag.IsDeactivated && !enemy.Bag.DeactivatedSoundPlayed) {
                        enemy.PlaySound(Sound.TurretDeactivate);
                        enemy.Bag.IsDeactivated = false;
                        enemy.Bag.DeactivatedSoundPlayed = true;
                    }
                },
                OnDie = (enemy) => {
                    enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position + Utility.RandomVector() * 200, Type = ParticleEffect.SmallExplosion });
                    enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position + Utility.RandomVector() * 200, Type = ParticleEffect.SmallExplosion });

                    if (Utility.Roll4())
                        enemy.Drop(MechanicalLoot(enemy.Tier), 1);

                    if (Utility.Roll8())
                        enemy.Drop(ItemId.RifleBarrel, 1);

                    if (Utility.Roll32())
                        enemy.Drop(ItemId.PhotovoltaicCell, 1);
                },
            });

            var floatingTurret = EnemyBase.Get("Turret").Clone();
            floatingTurret.Id = 9;
            floatingTurret.Name = "Floating Turret";
            floatingTurret.Description = "A turret that floats";
            floatingTurret.SoundName = "Turret";
            floatingTurret.IsFlying = true;
            EnemyBase.AddItem(floatingTurret);

            var grenadeTurret = EnemyBase.Get("Turret").Clone();
            grenadeTurret.Id = 18;
            grenadeTurret.Name = "Grenade Turret";
            grenadeTurret.Description = "A turret that lobs grenades";
            grenadeTurret.SoundName = "Turret";
            grenadeTurret.CoolDown = 1500;
            grenadeTurret.AlternateTextureName = "Turret Grenade Launcher";
            grenadeTurret.OnLongThink = (enemy) => {
                if (enemy.Target != null)
                    enemy.LobObjectAtTarget(ProjectileId.Grenade_2);
            };
            grenadeTurret.OnDie = (enemy) => {
                if (Utility.Flip())
                    enemy.Drop(ItemId.GrenadeBarrel);
            };
            EnemyBase.AddItem(grenadeTurret);

            var iceTurret = EnemyBase.Get("Turret").Clone();
            iceTurret.Id = 34;
            iceTurret.Name = "Ice Turret";
            iceTurret.ShootingOrigin = new Vector2(0, -13);
            iceTurret.ShootingOffset = 35;
            iceTurret.Description = "A turret that shoots ice";
            iceTurret.SoundName = "Turret";
            iceTurret.IsFlying = true;
            iceTurret.IsFlyingHeightFixed = true;
            iceTurret.Speed = 0;
            iceTurret.IsAutoSpawn = false;
            iceTurret.OnLongThink = (enemy) => {
                if (enemy.CooldownCheck(3000)) {
                    enemy.Delay(125, () => {
                        enemy.ShootAtAngle(ProjectileId.BoseEinsteinCondenser, (float)(3 * (Math.PI / 2)), false);//up
                    });
                    enemy.Delay(250, () => {
                        enemy.ShootAtAngle(ProjectileId.BoseEinsteinCondenser, (float)(7 * (Math.PI / 4)), false);//right up
                    });
                    enemy.Delay(375, () => {
                        enemy.ShootAtAngle(ProjectileId.BoseEinsteinCondenser, 0, false);//right
                    });
                    enemy.Delay(500, () => {
                        enemy.ShootAtAngle(ProjectileId.BoseEinsteinCondenser, (float)(Math.PI / 4), false);//right down
                    });
                    enemy.Delay(625, () => {
                        enemy.ShootAtAngle(ProjectileId.BoseEinsteinCondenser, (float)(Math.PI / 2), false);//down
                    });
                    enemy.Delay(750, () => {
                        enemy.ShootAtAngle(ProjectileId.BoseEinsteinCondenser, (float)(3 * (Math.PI / 4)), false);//left down
                    });
                    enemy.Delay(875, () => {
                        enemy.ShootAtAngle(ProjectileId.BoseEinsteinCondenser, (float)Math.PI, false);//left
                    });
                    enemy.Delay(1000, () => {
                        enemy.ShootAtAngle(ProjectileId.BoseEinsteinCondenser, (float)(5 * (Math.PI / 4)), false);//Left up
                    });
                }
            };
            EnemyBase.AddItem(iceTurret);

            EnemyBase.AddItem(new EnemyType {
                Id = 4,
                Name = "Exploder",
                Description = "Walks twards players and explodes when close",
                SpriteWidth = 72,
                SpriteHeight = 67,
                SpriteFramesPerRow = 3,
                MaxHealth = 60,
                Speed = 50,
                CollisionDamage = 5,
                CollisionKnockback = 500,
                IsAutoSpawn = true,
                MinSpawnTier = 3,
                SpawnRateLimitInSeconds = 180,
                OnSpawn = (enemy, tier) => {
                    enemy.Bag.ExplosionAge = 0.0;
                    enemy.IsFacingLeft = Utility.Flip();
                    enemy.SetAnimation(Animation.Move);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked)
                        enemy.SetAnimation(Animation.None);
                    else
                        enemy.SetAnimation(Animation.Move);
                    enemy.WalkTowardTarget();

                    if (enemy.Bag.ExplosionAge != 0.0) {
                        if (enemy.Bag.ExplosionAge < enemy.Age) {
                            enemy.Map.Explode(enemy.Position, 50);
                            enemy.Remove();
                        }
                    } else {
                        if (enemy.IsTargetCloserThan(200)) {
                            enemy.PlayingSound = Sound.ExploderWarning;
                            enemy.Bag.ExplosionAge = enemy.Age + 2000;
                        }
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);

                    if (Utility.Roll32())
                        enemy.Drop(ItemId.PhotovoltaicCell, 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 5,
                Name = "Floater",
                Description = "Floats in a line.",
                SpriteWidth = 44,
                SpriteHeight = 27,
                MaxHealth = 999,
                Speed = 75,
                Defense = Double.MaxValue,
                IsFlying = true,
                IsFlyingHeightFixed = true,
                CollisionDamage = 11,
                CollisionKnockback = 500,
                SpriteFramesPerRow = 4,
                IsAutoSpawn = true,
                MinSpawnTier = 5,
                SpawnRateLimitInSeconds = 180,
                OnSpawn = (enemy, tier) => {
                    if (Utility.Flip()) {
                        enemy.SetAnimation(Animation.Move);
                        enemy.Direction = 0;
                    } else {
                        enemy.SetAnimation(Animation.MoveBackwards);
                        enemy.Direction = (float)Math.PI;
                    }
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked) {
                        if (enemy.Direction != 0) {
                            enemy.SetAnimation(Animation.TurnRight);
                            enemy.Delay(150, () => {
                                enemy.SetAnimation(Animation.Move);
                                enemy.Direction = 0;
                            });
                        } else {
                            enemy.SetAnimation(Animation.TurnLeft);
                            enemy.Delay(150, () => {
                                enemy.SetAnimation(Animation.MoveBackwards);
                                enemy.Direction = (float)Math.PI;
                            });
                        }
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return 0;
                        case Animation.TurnLeft:
                            return Math.Min(age / 50, 3) + 1;
                        case Animation.MoveBackwards:
                            return 4;
                        case Animation.TurnRight:
                            return Math.Min(age / 50, 3) + 5;
                        default:
                            return 0;
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 6,
                Name = "Brunnen",
                Description = "Large enemy that walks twards players",
                SpriteWidth = 94,
                SpriteHeight = 107,
                SpriteFramesPerRow = 3,
                AlternateTextureName = "Brunnen Gun",
                ShootingOrigin = new Vector2(-2, -22),
                ShootingOffset = 55,
                MaxHealth = 107,
                Speed = 100,
                CollisionDamage = 20,
                CollisionKnockback = 500,
                IsAutoSpawn = true,
                MinSpawnTier = 2,
                SpawnRateLimitInSeconds = 180,
                AvoidsCliffs = true,
                DeathAnimationLengthInSeconds = 1,
                OnSpawn = (enemy, tier) => {
                    enemy.IsFacingLeft = Utility.Flip();
                    enemy.SetAnimation(Animation.Move);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnDrawTop = (enemy, spriteBatch, position, frame) => {
                    spriteBatch.Draw(enemy.Type.AlternateTexture, position + new Vector2(enemy.IsFacingLeft ? 2 : -2, -22), null, Color.White, enemy.Direction, new Vector2(enemy.Type.AlternateTexture.Width / 2, enemy.Type.AlternateTexture.Height / 2), 1, SpriteEffects.None, 0);
                },
                OnLongThink = (enemy) => {
                    enemy.Direction = enemy.GetShootingAngle();

                    if (enemy.IsBlocked && enemy.Target == null)
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;

                    //Stay next to player
                    if (!enemy.IsTargetCloserThan(600)) {
                        enemy.WalkTowardTarget();
                        enemy.Speed = 100;
                        if (enemy.IsBlocked)
                            enemy.SetAnimation(Animation.Idle1);
                        else
                            enemy.SetAnimation(Animation.MoveBackwards);
                    } else if (enemy.IsTargetCloserThan(500)) {
                        enemy.WalkAwayFromTarget();
                        enemy.Speed = 150;
                        if (enemy.IsBlocked)
                            enemy.SetAnimation(Animation.Idle2);
                        else
                            enemy.SetAnimation(Animation.Move);//moonwalk
                    } else {
                        enemy.Speed = 0;
                        if (enemy.Animation == Animation.MoveBackwards) {
                            enemy.SetAnimation(Animation.Idle1);
                        }

                        if (enemy.Animation == Animation.Move) {
                            enemy.SetAnimation(Animation.Idle2);
                        }
                    }

                    if (enemy.Target != null && enemy.CooldownCheck(900)) {
                        if (enemy.Target.IsJumping) {
                            float shootingOffset;
                            //if (Math.Abs(enemy.Direction) < (float)Math.PI  || Math.Abs(enemy.Direction) > (float)(2 * Math.PI))
                            if (enemy.IsFacingLeft)
                                shootingOffset = -.3f;
                            else
                                shootingOffset = .3f;

                            enemy.Direction += shootingOffset;
                            enemy.ShootAtTarget(ProjectileId.LaserRifle, shootingOffset);
                            if (Utility.Roll4()) {
                                enemy.Delay(100, () => {
                                    enemy.Direction += shootingOffset;
                                    enemy.ShootAtTarget(ProjectileId.LaserRifle, shootingOffset);
                                });
                            }
                        } else {
                            enemy.ShootAtTarget(ProjectileId.LaserRifle);
                            if (Utility.Roll4()) {
                                enemy.Delay(100, () => {
                                    enemy.ShootAtTarget(ProjectileId.LaserRifle);
                                });
                            }
                        }

                    } else if (enemy.Target != null && enemy.IsTargetCloserThan(280) && enemy.CooldownCheck(75)) {
                        enemy.ShootAtTarget(ProjectileId.Disruptor);
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.MoveBackwards:
                            return age / 100 % 9;
                        case Animation.Move:
                            return (age / 100 % 9) + 9;
                        case Animation.Idle1:
                            return 3;
                        case Animation.Idle2:
                            return 14;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    enemy.Delay(125, () => {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, enemy.Direction, false);
                        enemy.Direction += (float)(Math.PI / 4);
                    });
                    enemy.Delay(250, () => {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, enemy.Direction, false);
                        enemy.Direction += (float)(Math.PI / 4);
                    });
                    enemy.Delay(375, () => {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, enemy.Direction, false);
                        enemy.Direction += (float)(Math.PI / 4);
                    });
                    enemy.Delay(500, () => {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, enemy.Direction, false);
                        enemy.Direction += (float)(Math.PI / 4);
                    });
                    enemy.Delay(625, () => {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, enemy.Direction, false);
                        enemy.Direction += (float)(Math.PI / 4);
                    });
                    enemy.Delay(750, () => {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, enemy.Direction, false);
                        enemy.Direction += (float)(Math.PI / 4);
                    });
                    enemy.Delay(875, () => {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, enemy.Direction, false);
                        enemy.Direction += (float)(Math.PI / 4);
                    });
                    enemy.Delay(1000, () => {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, enemy.Direction, false);
                        enemy.Direction += (float)(Math.PI / 4);
                    });

                    if (Utility.Flip())
                        enemy.Drop(ItemId.Energy, Utility.Next(enemy.Tier) + 1);
                    else
                        enemy.Drop(ItemId.Ectoplasm, Utility.Next(enemy.Tier) + 1);

                    if (Utility.Roll16())
                        enemy.Drop(ItemId.OcularLens, 1);
                    if (Utility.Roll16())
                        enemy.Drop(ItemId.PhotovoltaicCell, 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 7,
                Name = "Leaper",
                Description = "Jumps at player",
                SpriteWidth = 103,
                SpriteHeight = 71,
                SpriteFramesPerRow = 5,
                RenderOffset = new Point(-7, 0),
                BoundingBox = new Rectangle(-35, -35, 60, 71),
                MaxHealth = 10,
                Speed = 100,
                CollisionDamage = 20,
                CollisionKnockback = 500,
                IsAutoSpawn = true,
                MinSpawnTier = 2,
                OnSpawn = (enemy, tier) => {
                    enemy.IsFacingLeft = Utility.Flip();
                    enemy.SetAnimation(Animation.Move);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                    enemy.Bag.IsJumping = false;
                },
                OnThink = (enemy) => {
                    if (enemy.Bag.IsJumping) {
                        enemy.Bag.IsJumping = false;
                        enemy.Delay(250, () => {
                            enemy.SetAnimation(Animation.Jump);
                            enemy.Velocity.Y = -1000;
                        });
                        enemy.Speed = 10;
                        enemy.SetAnimation(Animation.Attack1, true, 240);
                    }

                    if (enemy.Animation == Animation.Jump) {
                        enemy.Delay(250, () => {
                            enemy.Speed = 100;
                            enemy.SetAnimation(Animation.Move);
                        });
                        enemy.Speed = 10;
                        enemy.SetAnimation(Animation.Land, true, 240);
                    }
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked)
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;

                    if (Utility.Roll8() && enemy.IsOnGround)
                        enemy.Bag.IsJumping = true;

                    if (enemy.Target != null)
                        enemy.WalkTowardTarget();
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move://run
                            return age / 100 % 9;
                        case Animation.Attack1://Get ready to jump
                            var leapFrame = age / 80 % 3;
                            return leapFrame + 8;
                        case Animation.Jump://hold last frame
                            var jumpFrame = Math.Min(age / 150, 3);
                            if (jumpFrame > 1)
                                jumpFrame = 2;
                            return jumpFrame + 12;
                        case Animation.Land:
                            //var landFrame = age / 100 % 3;
                            //if (landFrame > 1)
                            //{
                            //    landFrame = 1;
                            //}

                            return (age / 80 % 3) + 14;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.Thermistor, Utility.Next(enemy.Tier) + 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 8,
                Name = "Crystalline",
                Description = "Spawns smaller creatures when killed",
                SpriteWidth = 100,
                SpriteHeight = 72,
                SpriteFramesPerRow = 4,
                MaxHealth = 25,
                Speed = 50,
                CollisionDamage = 11,
                CollisionKnockback = 500,
                IsAutoSpawn = true,
                MinSpawnTier = 2,
                AvoidsCliffs = true,
                OnSpawn = (enemy, tier) => {
                    enemy.IsFacingLeft = Utility.Flip();
                    enemy.SetAnimation(Animation.Attack2);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked) {
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;
                    }
                    if (enemy.Target != null) {
                        enemy.WalkTowardTarget();
                        if (enemy.IsTargetCloserThan(420)) {
                            enemy.SetAnimation(Animation.Attack1);
                            enemy.Speed = 160;
                        } else {
                            enemy.SetAnimation(Animation.Attack2);
                            enemy.Speed = 50;
                        }
                    } else {
                        if (Utility.Roll4()) {
                            enemy.SetAnimation(Animation.Attack2);
                            enemy.Speed = 50;
                            if (Utility.Roll64())
                                enemy.IsFacingLeft = !enemy.IsFacingLeft;
                        } else {
                            enemy.SetAnimation(Animation.Idle1);
                            enemy.Speed = 0;
                        }
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Attack1://run
                            return age / 50 % 9;
                        case Animation.Attack2://walk
                            return age / 120 % 9;
                        case Animation.Idle1://standing
                            return Utility.Oscillate(age / 50, 6) + 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    var crystallineType = EnemyBase.Get("Crystalline");
                    if (!Utility.Roll(6))
                        crystallineType = EnemyBase.Get("Crystalline Larva");

                    var crystalline = enemy.Map.AddEnemy(crystallineType, enemy.Position);
                    crystalline.Velocity += new Vector2(200, -500);
                    crystalline = enemy.Map.AddEnemy(crystallineType, enemy.Position);
                    crystalline.Velocity += new Vector2(0, -500);
                    crystalline = enemy.Map.AddEnemy(crystallineType, enemy.Position);
                    crystalline.Velocity += new Vector2(-200, -500);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 10,
                Name = "Crystalline Larva",
                SpriteWidth = 66,
                SpriteHeight = 39,
                SpriteFramesPerRow = 3,
                MaxHealth = 5,
                Speed = 25,
                CollisionDamage = 5,
                CollisionKnockback = 200,
                IsAutoSpawn = true,
                MinSpawnTier = 2,
                AvoidsCliffs = true,
                OnSpawn = (enemy, tier) => {
                    enemy.IsFacingLeft = Utility.Flip();
                    enemy.SetAnimation(Animation.Move);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked)
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;
                    if (enemy.Target != null)
                        enemy.WalkTowardTarget();
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm);
                    else
                        enemy.Drop(ItemId.Energy);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);

                    if (Utility.Roll4()) {
                        var loot = Utility.Next(3);
                        if (loot == 0)
                            enemy.Drop(ItemId.RedCrystal);
                        if (loot == 1)
                            enemy.Drop(ItemId.BlueCrystal);
                        if (loot == 2)
                            enemy.Drop(ItemId.GreenCrystal);
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 11,
                Name = "Montra Ray",
                Description = "Swims through water and surfaces to shoot at player",
                SpriteWidth = 91,
                SpriteHeight = 43,
                SpriteFramesPerRow = 5,
                RotateRender = true,
                IsRotationFixed = true,
                UpperRotationBounds = (float)(Math.PI / 6),
                LowerRotationBounds = (float)(5 * Math.PI / 6),
                MaxHealth = 17,
                Speed = 100,
                CollisionDamage = 11,
                CollisionKnockback = 500,
                IsHoming = true,
                IsSwimming = true,
                IsAutoSpawn = true,
                CoolDown = 150,
                OnSpawn = (enemy, tier) => {
                    enemy.SetAnimation(Animation.Move);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                    if (Utility.Flip())
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;

                    if (enemy.IsFacingLeft)
                        enemy.Direction = (float)Math.PI;
                    else
                        enemy.Direction = 0;
                },
                OnLongThink = (enemy) => {

                    if (enemy.Target != null) {
                        //enemy.SetAngleToTarget();
                        enemy.CheckFacingDirection();
                    }
                    //else if(Utility.Roll8())
                    //enemy.SetRandomAngle();

                    if (enemy.IsBlocked) {
                        enemy.Speed = 50;
                        if (enemy.Animation != Animation.TurnLeft) {
                            enemy.SetAnimation(Animation.TurnLeft, true, 320);
                            enemy.Delay(160, () => {
                                enemy.CheckFacingDirection();
                                enemy.Direction = MathHelper.WrapAngle(enemy.Direction + (float)Math.PI);
                            });
                            enemy.Delay(321, () => {
                                enemy.SetAnimation(Animation.Move);
                                enemy.Speed = 100;
                            });
                        }
                    }

                    if (enemy.Target != null && enemy.IsTargetCloserThan(420) && !MaterialInfo.IsLiquid(enemy.Map.GetMaterialAtPixel(enemy.FacePosition)))
                        enemy.ShootAtTarget(ProjectileId.Blaster);
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 12;
                        case Animation.TurnLeft:
                            var holdLeft = Math.Min(age / 40, 8);
                            //    //if (holdLeft > 7)
                            //    //    holdLeft = 8;
                            //    //Console.WriteLine("{0} Frame ", holdLeft);
                            return holdLeft + 11;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm);
                    else
                        enemy.Drop(ItemId.Energy);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.Wiring, Utility.Next(enemy.Tier) + 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 12,
                Name = "Material Eater",
                Description = "Tunnels through terrain",
                SpriteWidth = 32,
                SpriteHeight = 32,
                MaxHealth = 17,
                Speed = 20,
                CollisionDamage = 11,
                CollisionKnockback = 500,
                IsFlying = true,
                IsAutoSpawn = true,
                MinSpawnTier = 2,
                SpawnRateLimitInSeconds = 300,
                PenetrateWalls = true,
                RotateRender = true,
                OnLongThink = (enemy) => {
                    //only eats solids and loose
                    if (MaterialInfo.IsLooseOrSolid(enemy.Map.GetMaterialAtPixel(enemy.FacePosition)))
                        enemy.Map.RenderBrush(enemy.FacePosition, Brush.Size6, Material.Air, 1);

                    if (enemy.Target != null && enemy.IsTargetCloserThan(420)) {
                        enemy.SetAngleToTarget();
                    } else {
                        if (Utility.Roll16())
                            enemy.SetRandomAngle();
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.GunnDiode, Utility.Next(enemy.Tier) + 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 16,
                Name = "Dirt Creator",
                SoundName = "Material Eater",
                Description = "Creates a trail of dirt",
                SpriteWidth = 32,
                SpriteHeight = 32,
                MaxHealth = 17,
                Speed = 20,
                CollisionDamage = 11,
                CollisionKnockback = 500,
                IsFlying = true,
                IsAutoSpawn = true,
                MinSpawnTier = 2,
                SpawnRateLimitInSeconds = 300,
                PenetrateWalls = true,
                RotateRender = true,
                OnLongThink = (enemy) => {
                    if (enemy.Map.GetMaterialAtPixel(enemy.FacePosition) == Material.Air)
                        enemy.Map.RenderBrush(enemy.BackPosition, Brush.Size6, Material.Dirt, 1);

                    if (enemy.Target != null && enemy.IsTargetCloserThan(420)) {
                        enemy.SetAngleToTarget();
                    } else {
                        if (Utility.Roll16())
                            enemy.SetRandomAngle();
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.GunnDiode, Utility.Next(enemy.Tier) + 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 13,
                Name = "Firefly",
                Description = "Glowing bug that follows player",
                SpriteWidth = 12,
                SpriteHeight = 12,
                BoundingBox = new Rectangle(-18, -18, 24, 24),
                MaxHealth = 1,
                Speed = 200,
                CollisionDamage = 0,
                CollisionKnockback = 0,
                IsFlying = true,
                //IsHoming = true,
                IsAutoSpawn = true,
                //SpawnRateLimitInSeconds = 60,
                MinSpawnTier = 2,
                PenetrateWalls = false,
                //RotateRender = true,
                //Light = Color.FromNonPremultiplied(255, 171, 7, 255),
                Light = Color.FromNonPremultiplied(233, 233, 135, 230),
                OnLongThink = (enemy) => {
                    if (enemy.Target != null && enemy.IsTargetCloserThan(200)) {
                        enemy.SetAngleToTarget();
                    }

                    if (enemy.Target != null && (!enemy.IsTargetCloserThan(201) && enemy.IsTargetCloserThan(500))) {
                        if (Utility.Roll8())
                            enemy.SetRandomAngle();
                    }

                    if (enemy.IsDead) {
                        if (enemy.TargetEnemy != null)
                            enemy.SetAngleToTargetEnemy();
                        return;
                    }

                    if (enemy.Target == null || !enemy.IsTargetCloserThan(501)) {
                        enemy.Health = enemy.MaxHealth;

                        if (Utility.Roll8() || enemy.IsBlocked) {
                            enemy.SetRandomAngle();
                        }

                        if (Utility.Roll8()) // clear target placeable occasionally
                        {
                            enemy.TargetEnemy = null;
                        }

                        if (enemy.Target == null) {
                            var target = enemy.Map.FindTargetEnemy(enemy.Position, "Firefly");
                            enemy.TargetEnemy = target;
                        } else {
                            enemy.SetAngleToTargetEnemy();
                        }
                        enemy.Velocity = Vector2.Zero;
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Energy, Utility.Next(2) + 1);
                    else
                        enemy.Drop(ItemId.Ectoplasm, Utility.Next(2) + 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 14,
                Name = "Tall Valee Hive",
                Description = "Spawns a swarm when hit.",
                SpriteWidth = 86,
                SpriteHeight = 93,
                SpriteFramesPerRow = 3,
                MaxHealth = 30,
                Speed = 0,
                CollisionDamage = 5,
                CollisionKnockback = 500,
                OnSpawn = (enemy, tier) => {
                    enemy.SetAnimation(Animation.Idle1);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnHit = (enemy, amount) => {
                    if (Utility.Flip()) {
                        var valeeType = EnemyBase.Get("Valee");
                        enemy.Map.AddEnemy(valeeType, enemy.Position + new Vector2(0, -44));
                        enemy.Map.AddEnemy(valeeType, enemy.Position + new Vector2(0, -44));
                        enemy.Map.AddEnemy(valeeType, enemy.Position + new Vector2(0, -44));
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Idle1:
                            var currentFrame = (age / 100 % 9);
                            return currentFrame;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.FieldEffectTransistor, Utility.Next(enemy.Tier) + 1);
                },
            });

            var shortValeeHive = EnemyBase.Get("Tall Valee Hive").Clone();
            shortValeeHive.Id = 36;
            shortValeeHive.Name = "Short Valee Hive";
            shortValeeHive.SpriteWidth = 86;
            shortValeeHive.SpriteHeight = 76;
            shortValeeHive.BoundingBox = default(Rectangle); // so that it will get recreated
            EnemyBase.AddItem(shortValeeHive);

            EnemyBase.AddItem(new EnemyType {
                Id = 15,
                Name = "Valee",
                Description = "A swarming insect",
                SpriteWidth = 30,
                SpriteHeight = 39,
                SpriteFramesPerRow = 3,
                //BoundingBox = new Rectangle(-18, -18, 24, 24),
                MaxHealth = 1,
                Speed = 250,
                CollisionDamage = 5,
                CollisionKnockback = 0,
                IsFlying = true,
                IsHoming = true,
                PenetrateWalls = false,
                IsAutoSpawn = true,
                MinSpawnTier = 2,
                SpawnRateLimitInSeconds = 60,
                OnSpawn = (enemy, tier) => {
                    enemy.SetAnimation(Animation.Move);
                    enemy.SetRandomAngle();
                },
                OnLongThink = (enemy) => {
                    enemy.CheckFacingDirection();

                    if (Utility.Flip())
                        enemy.TurningRateInRadiansPerSecond = 8f;
                    else
                        enemy.TurningRateInRadiansPerSecond = 6f;

                    if (MaterialInfo.IsLiquid(enemy.Map.GetMaterialAtPixel(enemy.FacePosition))) {
                        enemy.Target = null;
                        enemy.TargetEnemy = null;
                        enemy.Damage(1);
                        return;
                    }

                    if (Utility.Roll8()) { // clear target placeable occasionall
                        //enemy.Target = null;
                        enemy.TargetEnemy = null;
                    }

                    if(enemy.IsBlocked){
                        enemy.Direction = enemy.Direction + (float)Math.PI;
                        enemy.CheckFacingDirection();
                    }
                    
                    if (enemy.Target == null || !enemy.IsTargetCloserThan(721)) {
                        enemy.Health = enemy.MaxHealth;
                            
                        var target = enemy.Map.FindTargetEnemy(enemy.Position, "Tall Valee Hive");
                        if (target == null)
                            target = enemy.Map.FindTargetEnemy(enemy.Position, "Short Valee Hive");
                        if (target == null)
                            target = enemy.Map.FindTargetEnemy(enemy.Position, "Valee");
                        enemy.TargetEnemy = target;
                        //enemy.Velocity = Vector2.Zero;
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 8 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Roll4()) {
                        if (Utility.Flip())
                            enemy.Drop(ItemId.Ectoplasm, 1);
                        else
                            enemy.Drop(ItemId.Energy, 1);

                        if (Utility.Roll4())
                            enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                        if (Utility.Roll4())
                            enemy.Drop(ItemId.Thyratron, Utility.Next(enemy.Tier) + 1);
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 17,
                Name = "Mind Feeder",
                Description = "Hunts down players and latches onto their heads",
                SpriteWidth = 38,
                SpriteHeight = 30,
                SpriteFramesPerRow = 5,
                MaxHealth = 17,
                Speed = 200,
                CollisionDamage = 4,
                CollisionKnockback = 0,
                IsFlying = true,
                IsHoming = true,
                MinSpawnTier = 4,
                IsAutoSpawn = true,
                OnSpawn = (enemy, tier) => {
                    enemy.SetRandomAngle();
                    enemy.Bag.LatchedOntoTarget = false;
                    enemy.SetAnimation(Animation.Idle1);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnThink = (enemy) => {
                    if (enemy.Bag.LatchedOntoTarget) {
                        if (enemy.Target == null) {
                            enemy.Bag.LatchedOntoTarget = false;
                            enemy.Target = null;
                        } else {
                            enemy.Position = enemy.Target.Position + new Vector2(0, -38);
                            if (enemy.Target.IsFacingLeft)
                                enemy.IsFacingLeft = true;
                            else
                                enemy.IsFacingLeft = false;
                        }
                    }
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked)
                        enemy.SetRandomAngle();

                    if (!enemy.Bag.LatchedOntoTarget && !enemy.IsDead) {
                        if (enemy.Target != null && enemy.IsTargetCloserThan(420)) {
                            //enemy.SetAngleToTarget();
                            enemy.SetAnimation(Animation.Hovering);
                        }

                        if (enemy.IsTargetCloserThan(50)) {
                            enemy.Bag.LatchedOntoTarget = true;
                            enemy.SetAnimation(Animation.LatchedOn);
                        } else {
                            if (Utility.Roll16()) {
                                enemy.SetRandomAngle();
                                enemy.SetAnimation(Animation.Idle1);
                            }
                        }
                    }
                },
                OnDie = (enemy) => {
                    enemy.Bag.LatchedOntoTarget = false;

                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.Thermistor, Utility.Next(enemy.Tier) + 1);

                    if (Utility.Roll32())
                        enemy.Drop(ItemId.PhotovoltaicCell, 1);
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.LatchedOn:
                            return Utility.Oscillate(age / 100, 4);
                        case Animation.Hovering:
                            return Utility.Oscillate(age / 100, 3) + 4;
                        case Animation.Idle1:
                            return Utility.Oscillate(age / 100, 3) + 7;
                        default:
                            return 0;
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 19,
                Name = "Spitter",
                SoundName = "Floater",
                Description = "Shoots LEFT",
                SpriteWidth = 32,
                SpriteHeight = 32,
                MaxHealth = 45,
                Defense = Double.MaxValue,
                CollisionDamage = 10,
                CollisionKnockback = 500,
                CanSeeThruWalls = true,
                IsFlying = true,
                CoolDown = 1000,
                OnSpawn = (enemy, tier) => {
                    enemy.Direction = (float)Math.PI;
                },
                OnThink = (enemy) => {
                    if (enemy.Target != null && enemy.IsTargetCloserThan(1000)) {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, (float)Math.PI);
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 20,
                Name = "Spitter",
                SoundName = "Floater",
                Description = "Shoots RIGHT",
                SpriteWidth = 32,
                SpriteHeight = 32,
                MaxHealth = 45,
                Defense = Double.MaxValue,
                CollisionDamage = 10,
                CollisionKnockback = 500,
                CanSeeThruWalls = true,
                IsFlying = true,
                CoolDown = 1000,
                OnSpawn = (enemy, tier) => {
                    enemy.Direction = 0.0F;
                },
                OnThink = (enemy) => {
                    if (enemy.Target != null && enemy.IsTargetCloserThan(1000)) {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, 0);
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 21,
                Name = "Spitter",
                SoundName = "Floater",
                Description = "Shoots UP",
                SpriteWidth = 32,
                SpriteHeight = 32,
                MaxHealth = 45,
                Defense = Double.MaxValue,
                CollisionDamage = 10,
                CollisionKnockback = 500,
                CanSeeThruWalls = true,
                IsFlying = true,
                CoolDown = 1000,
                OnSpawn = (enemy, tier) => {
                    enemy.Direction = (float)(3 * (Math.PI / 2));
                },
                OnThink = (enemy) => {
                    if (enemy.Target != null && enemy.IsTargetCloserThan(1000)) {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, (float)(3 * (Math.PI / 2)));//up
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 22,
                Name = "Spitter",
                SoundName = "Floater",
                Description = "Shoots DOWN",
                SpriteWidth = 32,
                SpriteHeight = 32,
                MaxHealth = 45,
                Defense = Double.MaxValue,
                CollisionDamage = 10,
                CollisionKnockback = 500,
                CanSeeThruWalls = true,
                IsFlying = true,
                CoolDown = 1000,
                OnSpawn = (enemy, tier) => {
                    enemy.Direction = (float)(Math.PI / 2);
                },
                OnThink = (enemy) => {
                    if (enemy.Target != null && enemy.IsTargetCloserThan(1000)) {
                        enemy.ShootAtAngle(ProjectileId.LaserRifle, (float)(Math.PI / 2));//down
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 23,
                Name = "Sprout",
                Description = "Shoots spikes at player",
                SpriteWidth = 32,
                SpriteHeight = 32,
                MaxHealth = 7,
                Speed = 0,
                CollisionDamage = 0,
                CollisionKnockback = 0,
                MinSpawnTier = 2,
                IsAutoSpawn = true,
                OnSpawn = (enemy, tier) => {
                    enemy.TargetClosestPlayer();
                },
                OnThink = (enemy) => {
                    if (enemy.Target != null && enemy.CooldownCheck(1200) && enemy.IsTargetCloserThan(1000)) {
                        enemy.ShootAtAngle(ProjectileId.SproutSpike, (float)(3 * (Math.PI / 2)), false);//up
                        enemy.ShootAtAngle(ProjectileId.SproutSpike, 0, false);//right
                        enemy.ShootAtAngle(ProjectileId.SproutSpike, (float)Math.PI, false);//left
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    enemy.Drop(ItemId.Energy, Utility.Next(2));
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 24,
                Name = "Eel",
                SoundName = "Eel",
                Description = "Swims through water quickly and chases target.",
                SpriteWidth = 149,
                SpriteHeight = 68,
                SpriteFramesPerRow = 4,
                MaxHealth = 5,
                Defense = 20,
                Speed = 25,
                CollisionDamage = 6,
                CollisionKnockback = 10,
                IsHoming = true,
                IsSwimming = true,
                IsAutoSpawn = true,
                SpawnRateLimitInSeconds = 300,
                RotateRender = true,
                IsRotationFixed = true,
                //UpperRotationBounds = (float)(Math.PI / 4),
                //LowerRotationBounds = (float)(3 * (Math.PI / 4)),
                Light = Color.FromNonPremultiplied(200, 0, 0, 150),
                OnSpawn = (enemy, tier) => {
                    enemy.SetRandomAngle();
                    enemy.SetAnimation(Animation.Attack2);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                    enemy.TurningRateInRadiansPerSecond = 2f;
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked)
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;

                    if (enemy.Target != null) {
                        //enemy.SetAngleToTarget();
                        //enemy.CheckFacingDirection();

                        if (enemy.IsTargetCloserThan(420)) {
                            enemy.SetAnimation(Animation.Attack1);
                            enemy.Speed = 200;
                        }
                    } else {
                        if (Utility.Roll16() || enemy.IsBlocked)
                            enemy.SetRandomAngle();
                        enemy.SetAnimation(Animation.Attack2);
                        enemy.Speed = 25;
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Attack1://fast swim
                            return age / 80 % 9;
                        case Animation.Attack2://slow swim
                            return (age / 120 % 9) + 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.GunnDiode, Utility.Next(enemy.Tier) + 1);

                    if (Utility.Roll32())
                        enemy.Drop(ItemId.PhotovoltaicCell, 1);
                },
            });

            var smallEel = EnemyBase.Get("Eel").Clone();
            smallEel.Id = 35;
            smallEel.Name = "Small Eel";
            smallEel.Defense = 5;
            smallEel.SpriteWidth = 70;
            smallEel.SpriteHeight = 32;
            smallEel.Light = Color.FromNonPremultiplied(200, 0, 0, 150);
            smallEel.BoundingBox = default(Rectangle); // so that it will get recreated
            smallEel.CollisionDamage = 8;
            smallEel.OnSpawn = (enemy, tier) => {
                enemy.SetRandomAngle();
                enemy.SetAnimation(Animation.Attack2);
                enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                enemy.TurningRateInRadiansPerSecond = 4f;
            };
            EnemyBase.AddItem(smallEel);

            EnemyBase.AddItem(new EnemyType {
                Id = 25,
                Name = "Jelly Fish",
                //SoundName = "Eel",
                Description = "Swims slowly through water. Dangereous if come in contact.",
                SpriteWidth = 31,
                SpriteHeight = 39,
                SpriteFramesPerRow = 3,
                Defense = 1,
                MaxHealth = 25,
                Speed = 60,
                CollisionDamage = 60,
                CollisionKnockback = 1,
                IsSwimming = true,
                IsAutoSpawn = true,
                Light = Color.FromNonPremultiplied(202, 197, 255, 75),
                OnSpawn = (enemy, tier) => {
                    enemy.SetRandomAngle();
                    enemy.SetAnimation(Animation.Move, true);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnLongThink = (enemy) => {
                    if (Utility.Roll8())
                        enemy.SetRandomAngle();

                    if (enemy.IsBlocked)
                        enemy.SetRandomAngle();

                    enemy.Speed = 60 + (enemy.AnimationAge / 100 % 9);
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.FieldEffectTransistor, Utility.Next(enemy.Tier) + 1);

                    if (Utility.Roll32())
                        enemy.Drop(ItemId.PhotovoltaicCell, 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 26,
                Name = "Bomber",
                SoundName = "Drake",
                Description = "Sits on the ceiling until hero comes into view then dives at them. Dies after impact.",
                SpriteWidth = 32,
                SpriteHeight = 40,
                MaxHealth = 25,
                Speed = 0,
                CollisionDamage = 60,
                CollisionKnockback = 1,
                IsFlying = true,
                IsAutoSpawn = true,
                MinSpawnTier = 2,
                SpawnAtCeiling = true,
                OnSpawn = (enemy, tier) => {
                    enemy.Direction = (float)(Math.PI / 2);//down
                    enemy.SetAnimation(Animation.Hovering);
                    enemy.Bag.IsSound = false;
                },
                OnLongThink = (enemy) => {
                    if (enemy.Target != null) {
                        if (enemy.IsTargetCloserThan(400)) {
                            if (enemy.Bag.IsSound == false) {
                                enemy.PlaySound(Sound.BomberWarning, 0, 0);
                                enemy.Bag.IsSound = true;
                            }
                            enemy.SetAnimation(Animation.Attack1);
                        }
                    }

                    if (enemy.Animation == Animation.Attack1) {
                        enemy.Speed = 900;
                    }

                    if (enemy.IsOnGround) {
                        enemy.Health = 0;
                        enemy.ShootAtAngle(ProjectileId.Spike, (float)(3 * (Math.PI / 2)), false);//up
                        enemy.ShootAtAngle(ProjectileId.Spike, (float)(7 * (Math.PI / 4)), false);//right up
                        enemy.ShootAtAngle(ProjectileId.Spike, 0, false);//right
                        enemy.ShootAtAngle(ProjectileId.Spike, (float)(5 * (Math.PI / 4)), false);//Left up
                        enemy.ShootAtAngle(ProjectileId.Spike, (float)Math.PI, false);//left
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.Thyratron, Utility.Next(enemy.Tier) + 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 27,
                Name = "Ground Trooper",
                Description = "Standard Alien ground unit, has blaster",
                SpriteWidth = 67,
                SpriteHeight = 68,
                SpriteFramesPerRow = 3,
                AlternateTextureName = "Ground Trooper Gun",
                ShootingOrigin = new Vector2(-8, -17),
                ShootingOffset = 30,
                MaxHealth = 32,
                Speed = 75,
                CollisionDamage = 5,
                CollisionKnockback = 50,
                IsAutoSpawn = true,
                MinSpawnTier = 2,
                AvoidsCliffs = true,
                OnSpawn = (enemy, tier) => {
                    enemy.IsFacingLeft = Utility.Flip();
                    enemy.SetAnimation(Animation.Move);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                    enemy.Bag.TrooperShotFirst = true;
                },
                OnDrawTop = (enemy, spriteBatch, position, frame) => {
                    spriteBatch.Draw(enemy.Type.AlternateTexture, position + new Vector2(enemy.IsFacingLeft ? 8 : -8, -20), null, Color.White, enemy.Direction, new Vector2(enemy.Type.AlternateTexture.Width / 2, enemy.Type.AlternateTexture.Height / 2), 1, SpriteEffects.None, 0);
                },
                OnLongThink = (enemy) => {
                    enemy.Direction = enemy.GetShootingAngle();

                    if (enemy.IsBlocked)
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;

                    if ((float)enemy.Health / enemy.MaxHealth <= 0.5f) {
                        // run away injured
                        enemy.Speed = 30;
                        enemy.WalkAwayFromTarget();
                    } else if (enemy.IsTargetCloserThan(1200)) {
                        enemy.Speed = 100;
                        enemy.WalkTowardTarget();
                    } else
                        enemy.Speed = 10;

                    if (enemy.Target != null && enemy.Bag.TrooperShotFirst) {
                        enemy.ShootAtTarget(ProjectileId.Blaster);
                        enemy.Bag.TrooperShotFirst = false;
                    }

                    if (enemy.Target != null && Utility.Roll4()) {
                        //if (Utility.Flip()) {
                        //    enemy.ShootAtTarget(ProjectileId.BlasterCharged);
                        //} else
                        enemy.ShootAtTarget(ProjectileId.Blaster);
                    }

                    if (enemy.Target == null)
                        enemy.Bag.TrooperShotFirst = true;
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Flip())
                        enemy.Drop(ItemId.Ectoplasm, 1);
                    else
                        enemy.Drop(ItemId.Energy, 1);

                    if (Utility.Roll4())
                        enemy.Drop(MonsterLoot(), Utility.Next(enemy.Tier) + 1);
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.Thermistor, Utility.Next(enemy.Tier) + 1); ;
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 28,
                Name = "Point Defense Drone",
                SoundName = "Turret",
                Description = "Standard Alien patrol unit.",
                SpriteWidth = 24,
                SpriteHeight = 24,
                MaxHealth = 10,
                Speed = 50,
                CollisionDamage = 1,
                CollisionKnockback = 1,
                IsAutoSpawn = true,
                MinSpawnTier = 3,
                IsFlying = true,
                OnSpawn = (enemy, tier) => {
                    enemy.IsFacingLeft = Utility.Flip();
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked)
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;

                    //Stay next to player
                    if (!enemy.IsTargetCloserThan(300)) {
                        enemy.Speed = 175;
                        enemy.SetAngleToTarget();
                    }

                    if (enemy.IsTargetCloserThan(300)) {
                        enemy.Speed = 0;
                    }

                    if (enemy.IsTargetCloserThan(200)) {
                        enemy.Speed = 175;
                        enemy.SetAngleAwayFromTarget();
                    }

                    if (enemy.Target == null) {
                        enemy.SetRandomAngle();
                        enemy.Speed = 50;
                    }

                    if (enemy.IsTargetCloserThan(420) && enemy.CooldownCheck(900))
                        enemy.ShootAtTarget(ProjectileId.Blaster);

                    if (enemy.IsBlocked)
                        enemy.SetRandomAngle();
                },
                OnDie = (enemy) => {
                    enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position + Utility.RandomVector() * 200, Type = ParticleEffect.SmallExplosion });
                    enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position + Utility.RandomVector() * 200, Type = ParticleEffect.SmallExplosion });

                    if (Utility.Roll4())
                        enemy.Drop(MechanicalLoot(enemy.Tier), 1);

                    if (Utility.Roll8())
                        enemy.Drop(ItemId.RifleBarrel, 1);

                    if (Utility.Roll32())
                        enemy.Drop(ItemId.PhotovoltaicCell, 1);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 29,
                Name = "Light Drone",
                Description = "A drone that gives off light.",
                SpriteWidth = 16,
                SpriteHeight = 16,
                MaxHealth = 1,
                Speed = 400,
                CollisionDamage = 0,
                CollisionKnockback = 0,
                IsFlying = true,
                PenetrateWalls = false,
                RotateRender = true,
                Light = Color.Red,
                OnLongThink = (enemy) => {
                    if (enemy.Target != null && enemy.IsTargetCloserThan(400)) {
                        enemy.SetAngleToTarget();
                    } else {
                        if (enemy.IsDead) {
                            if (enemy.TargetPlaceable != null)
                                enemy.SetAngleToTargetPlaceable();
                            return;
                        }

                        if (enemy.Target == null || enemy.Target.IsDead) {
                            enemy.Health = enemy.MaxHealth;
                            enemy.SetRandomAngle();

                            if (enemy.IsOnGround || enemy.IsAgainstCeiling)
                                enemy.SetAngleAwayFromTarget();
                            if (enemy.IsBlocked)
                                enemy.SetAngleAwayFromTarget();
                        }
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.Iron, 2);
                    enemy.Drop(ItemId.Ectoplasm, Utility.Next(3));
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 30,
                Name = "Portal Seeker",
                Description = "Finds a specific portal based on what device built it.",
                SpriteWidth = 37,
                SpriteHeight = 25,
                SpriteFramesPerRow = 2,
                MaxHealth = 1,
                Defense = int.MaxValue,
                Speed = 200,
                CollisionDamage = 0,
                CollisionKnockback = 0,
                IsFlying = true,
                PenetrateWalls = true,
                RotateRender = true,
                Light = Color.FromNonPremultiplied(121, 253, 151, 250),
                OnSpawn = (enemy, tier) => {
                    enemy.SetAnimation(Animation.Move);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnLongThink = (enemy) => {
                    if (enemy.Age > 60 * 1000)
                        enemy.Remove();
                    else {
                        if (enemy.TargetPlaceable != null) {
                            enemy.SetAngleToTargetPlaceable();
                        } else
                            enemy.Speed = 0;
                    }
                },
                OnDraw = (enemy, spriteBatch, position, frame) => {
                    Particles.Add(new Particle() { Position = enemy.Position, Color = Color.FromNonPremultiplied(121, 253, 151, 250), Velocity = Utility.RandomVector() * 0.1f, MaxAge = 0.5, Scale = 0.5f });
                },
                GetFrame = (animation, age) => {
                    return Utility.Oscillate(age / 100, 4);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 32,
                Name = "Mining Drone",
                SoundName = "Light Drone",
                Description = "A short lived drone that mines.",
                SpriteWidth = 16,
                SpriteHeight = 16,
                MaxHealth = 30,
                Speed = 400,
                CollisionDamage = 0,
                CollisionKnockback = 0,
                IsFlying = true,
                PenetrateWalls = false,
                RotateRender = true,
                OnLongThink = (enemy) => {
                    enemy.Health--;

                    if (enemy.Target != null && enemy.IsTargetCloserThan(400)) {
                        enemy.SetAngleToTarget();

                        if (enemy.IsTargetCloserThan(100))
                            enemy.Speed = 0;
                        else
                            enemy.Speed = enemy.Type.Speed;

                        if (enemy.CooldownCheck(100)) {
                            var projectile = enemy.ShootAtAngle(ProjectileId.DigMedium, enemy.Target.GetShootingAngle(enemy.Target.GetShootingOrigin()));
                            enemy.Delay(10, () => {
                                projectile.Owner = enemy.Target;
                                projectile.IsPlayerOwned = true;
                            });
                        }

                        if (enemy.IsOnGround || enemy.IsAgainstCeiling)
                            enemy.SetAngleAwayFromTarget();
                        if (enemy.IsBlocked)
                            enemy.SetAngleAwayFromTarget();
                    } else {
                        if (enemy.IsDead) {
                            enemy.Health = 0;
                            if (enemy.TargetPlaceable != null)
                                enemy.SetAngleToTargetPlaceable();
                            return;
                        }

                        if (enemy.Target == null || enemy.Target.IsDead) {
                            enemy.Health = 0;
                        }
                    }
                },
                OnDie = (enemy) => {
                    if (Utility.Roll4())
                        enemy.Drop(ItemId.Gold, 1);
                },
            });

            //Bosses
            EnemyBase.AddItem(new EnemyType {
                Id = 100,
                Name = "Krawnix",
                SpriteWidth = 349,
                SpriteHeight = 304,
                SpriteFramesPerRow = 3,
                RenderOffset = new Point(-15, -39),
                BoundingBox = new Rectangle(-175, -65, 297, 220),
                MaxHealth = 400,
                CollisionDamage = 100,
                CollisionKnockback = 1000,
                IsBoss = true,
                DeathAnimationLengthInSeconds = 0.5,
                ShootingOrigin = new Vector2(118, 13),
                BloodColor = Color.FromNonPremultiplied(189, 240, 64, 255),
                CoolDown = 800,
                OnSpawn = (enemy, tier) => {
                    enemy.Bag.Spawned = new List<Enemy>();
                    enemy.SetAnimation(Animation.None, true);
                },
                OnLongThink = (enemy) => {
                    if (enemy.Target != null) {
                        enemy.CastLight = Color.FromNonPremultiplied(153, 206, 48, 255);
                        if (enemy.Animation == Animation.None && Utility.Roll4()) {
                            if (enemy.IsCooledDown()) {
                                if (Utility.Flip()) {
                                    enemy.Delay(300, () => enemy.ShootAtTarget(ProjectileId.AcidSpit));
                                    enemy.SetAnimation(Animation.Attack1, true, 599);
                                } else {
                                    enemy.Delay(300, () => enemy.ShootAtTarget(ProjectileId.AcidSpit));
                                    enemy.SetAnimation(Animation.Attack2, true, 599);
                                }
                            }
                        }

                        if (enemy.Animation == Animation.None) {
                            var spawnedCount = (from e in (List<Enemy>)enemy.Bag.Spawned where !e.IsDead select e).Count();
                            if (spawnedCount <= 6 && Utility.Roll8() && enemy.IsCooledDown()) {
                                enemy.SetAnimation(Animation.Attack3, true, 599);
                                enemy.Delay(300, () => {
                                    var crawlerType = EnemyBase.Get("Crawler");
                                    Enemy crawler;
                                    crawler = enemy.Map.AddEnemy(crawlerType, enemy.Position + new Vector2(80, -80));
                                    crawler.Velocity += new Vector2(300, -500);
                                    crawler.IsFacingLeft = false;
                                    crawler.CastLight = Color.FromNonPremultiplied(194, 245, 65, 200);
                                    enemy.Bag.Spawned.Add(crawler);

                                    crawler = enemy.Map.AddEnemy(crawlerType, enemy.Position + new Vector2(80, -80));
                                    crawler.Velocity += new Vector2(500, -500);
                                    crawler.IsFacingLeft = false;
                                    crawler.CastLight = Color.FromNonPremultiplied(194, 245, 65, 200);
                                    enemy.Bag.Spawned.Add(crawler);

                                    //crawler = enemy.Map.AddEnemy(crawlerType, enemy.Position + new Vector2(80, -80));
                                    //crawler.Velocity += new Vector2(700, -500);
                                    //crawler.IsFacingLeft = false;
                                    //crawler.CastLight = Color.FromNonPremultiplied(194, 245, 65, 200);
                                    //enemy.Bag.Spawned.Add(crawler);
                                });
                            }
                        }
                    } else {
                        enemy.CastLight = Color.Black;
                        enemy.Health = enemy.MaxHealth;
                    }
                },
                OnDie = (enemy) => {
                    enemy.Delay(400, () => {
                        enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position, Type = ParticleEffect.KrawnixExplode, Color = enemy.Type.BloodColor, Value = 2 });
                    });

                    //Loot
                    enemy.Drop(ItemId.Ectoplasm, 15);

                    //Reward Chest
                    var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), enemy.Position);
                    chest.AddItem(new Item() { TypeId = ItemId.SteelSuperconductor, Amount = 1 });
                    chest.AddItem(new Item() { TypeId = ItemId.LightOrb, Amount = 5 });
                    chest.AddItem(new Item() { TypeId = ItemId.Steel, Amount = 10 });
                    chest.AddItem(new Item() { TypeId = ItemId.EchoCrystal, Amount = 2 });
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.None:
                            return Utility.LongOscillate(age / 200, 3);
                        case Animation.Attack1:
                            return Utility.LongOscillate(age / 100, 3) + 3;
                        case Animation.Attack2:
                            return Utility.LongOscillate(age / 100, 3) + 6;
                        case Animation.Attack3: // spit
                            return Utility.LongOscillate(age / 100, 3) + 12;
                        case Animation.Dead:
                            if (age > 400)
                                return -1;
                            return Math.Min(age / 100, 2) + 9;
                        default:
                            return 0;
                    }
                },
                GetFrameHeightOverride = (frame) => { return frame > 11 ? 304 : 0; },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 109,
                Name = "Sky Realm Mini 1",
                Description = "Turret that shoots fire at platforms forcing the player to move",
                SpriteWidth = 71,
                SpriteHeight = 64,
                SpriteFramesPerRow = 5,
                AlternateTextureName = "Sky Realm Mini 1 Gun",
                ShootingOrigin = new Vector2(0, -33),
                ShootingOffset = 110,
                MaxHealth = 400,
                CollisionDamage = 100,
                CollisionKnockback = 1000,
                IsFlying = true,
                Speed = 0,
                IsBoss = true,
                CoolDown = 0,
                DeathAnimationLengthInSeconds = 0,
                OnSpawn = (enemy, tier) => {
                    enemy.Bag.Spawned = new List<Enemy>();
                    enemy.SetAnimation(Animation.None, true);
                    enemy.PlayingSound = Sound.None;
                },
                OnDrawTop = (enemy, spriteBatch, position, frame) => {
                    spriteBatch.Draw(enemy.Type.AlternateTexture, position + new Vector2(0, -36), null, Color.White, enemy.Direction, new Vector2(enemy.Type.AlternateTexture.Width / 2, enemy.Type.AlternateTexture.Height / 2), 1, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                },
                OnLongThink = (enemy) => {
                    enemy.Direction = enemy.GetShootingAngle();

                    if (enemy.Target == null)
                        enemy.PlayingSound = Sound.None;

                    if (enemy.Target != null && enemy.IsTargetCloserThan(680)) {
                        if ((float)enemy.Health / (float)enemy.MaxHealth >= .50) {
                            enemy.ShootAtTarget(ProjectileId.SlowBlaster);
                        } else if ((float)enemy.Health / (float)enemy.MaxHealth >= .25 && (float)enemy.Health / (float)enemy.MaxHealth <= .49) {
                            enemy.ShootAtTarget(ProjectileId.SlowBlaster, (float)-Math.PI / 18);
                            enemy.ShootAtTarget(ProjectileId.SlowBlaster);
                            enemy.ShootAtTarget(ProjectileId.SlowBlaster, (float)Math.PI / 18);
                        } else if (enemy.CooldownCheck(500)) {
                            enemy.PlayingSound = Sound.Static;
                            enemy.ShootAtTarget(ProjectileId.BallLightning);
                            if (Utility.Flip()) {
                                var shootingDir = (float)Utility.Next();
                                enemy.Direction = shootingDir;
                                enemy.ShootAtAngle(ProjectileId.BallLightning, shootingDir);
                                shootingDir = (float)Utility.Next();
                                enemy.Direction = shootingDir;
                                enemy.ShootAtAngle(ProjectileId.BallLightning, shootingDir);
                                //enemy.ShootAtAngle(ProjectileId.BallLightning, (float)Utility.Next());
                            }
                        }

                        if (enemy.CooldownCheck(5000)) {
                            enemy.Delay(125, () => {
                                enemy.Direction = (float)(3 * (Math.PI / 2));
                                enemy.ShootAtAngle(ProjectileId.LaserRifle, (float)(3 * (Math.PI / 2)));//up
                            });
                            enemy.Delay(250, () => {
                                enemy.Direction = (float)(7 * (Math.PI / 4));
                                enemy.ShootAtAngle(ProjectileId.LaserRifle, (float)(7 * (Math.PI / 4)));//right up
                            });
                            enemy.Delay(375, () => {
                                enemy.Direction = 0;
                                enemy.ShootAtAngle(ProjectileId.LaserRifle, 0);//right
                            });
                            enemy.Delay(500, () => {
                                enemy.Direction = (float)(Math.PI / 4);
                                enemy.ShootAtAngle(ProjectileId.LaserRifle, (float)(Math.PI / 4));//right down
                            });
                            enemy.Delay(625, () => {
                                enemy.Direction = (float)(Math.PI / 2);
                                enemy.ShootAtAngle(ProjectileId.LaserRifle, (float)(Math.PI / 2));//down
                            });
                            enemy.Delay(750, () => {
                                enemy.Direction = (float)(3 * (Math.PI / 4));
                                enemy.ShootAtAngle(ProjectileId.LaserRifle, (float)(3 * (Math.PI / 4)));//left down
                            });
                            enemy.Delay(875, () => {
                                enemy.Direction = (float)Math.PI;
                                enemy.ShootAtAngle(ProjectileId.LaserRifle, (float)Math.PI);//left
                            });
                            enemy.Delay(1000, () => {
                                enemy.Direction = (float)(5 * (Math.PI / 4));
                                enemy.ShootAtAngle(ProjectileId.LaserRifle, (float)(5 * (Math.PI / 4)));//Left up
                            });
                        }
                    }
                    //else
                    //    enemy.Health = enemy.MaxHealth;
                },
                OnDie = (enemy) => {
                    //enemy.SetAnimation(Animation.Dead);
                    enemy.PlayingSound = Sound.None;
                    enemy.Map.Explode(enemy.Position, 15);

                    //Loot
                    var LootLocation = enemy.Map.FindPlaceables("TurretChest").FirstOrDefault();
                    var bossDoor = enemy.Map.FindPlaceables("bossDoor").FirstOrDefault();

                    if (bossDoor != null)
                        bossDoor.IsSolid = false;

                    if (LootLocation != null) {
                        var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), LootLocation.Position);
                        chest.AddItem(new Item() { TypeId = ItemId.Gold, Amount = 10 });
                        chest.AddItem(new Item() { TypeId = ItemId.LightOrb, Amount = 10 });
                        chest.AddItem(new Item() { TypeId = ItemId.Emerald, Amount = 7 });
                        chest.AddItem(new Item() { TypeId = ItemId.MajorEnergyCell, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.EchoCrystal, Amount = 1 });
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return Math.Min(age / 100, 4);
                        default:
                            return 0;
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 107,
                Name = "Sky Realm Mini 2",
                Description = "Big Fat slime monster. Sits in the slime and shoots poison darts. Submerges in slime to become un-targetable and spawns 3-4 flyers. After flyers are dead or after a short time the slime monster reemerges and begins attacking again.",
                SpriteWidth = 64,
                SpriteHeight = 42,
                SpriteFramesPerRow = 3,
                MaxHealth = 200,
                Speed = 100,
                CollisionDamage = 20,
                CollisionKnockback = 500,
                IsHoming = true,
                IsSwimming = true,
                IsBoss = true,
                ShootingOrigin = new Vector2(0, -32),
                OnSpawn = (enemy, tier) => {
                    enemy.Bag.Spawned = new List<Enemy>();
                    enemy.SetRandomAngle();
                    enemy.SetAnimation(Animation.Attack1);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnLongThink = (enemy) => {
                    if (enemy.Target != null && enemy.AnimationAge > 6000 && enemy.Animation != Animation.Attack1) {
                        enemy.SetAnimation(Animation.Attack1);
                    }

                    if (enemy.Animation == Animation.Attack1) {
                        if (enemy.Target != null)
                            //enemy.SetAngleToTarget();
                            enemy.CheckFacingDirection();

                        if (enemy.Target != null && !MaterialInfo.IsLiquid(enemy.Map.GetMaterialAtPixel(enemy.FacePosition)) && enemy.CooldownCheck(900))
                            enemy.ShootAtTarget(ProjectileId.PoisonDart);
                        if (enemy.AnimationAge > 6000)
                            enemy.SetAnimation(Animation.None);
                    }

                    if (enemy.Animation == Animation.None) {

                        if (enemy.IsBlocked || !MaterialInfo.IsLiquid(enemy.Map.GetMaterialAtPixel(enemy.FacePosition))) {
                            var newDir = Utility.RandomAngle(180);
                            enemy.Direction = newDir;
                        } else {
                            if (enemy.TargetPlaceable == null) {
                                var targets = enemy.Map.FindPlaceables("Slime").ToArray();
                                if (targets.Length > 0)
                                    enemy.TargetPlaceable = targets[Utility.Next(targets.Length)];
                            } else {
                                if (enemy.TargetPlaceable == null)
                                    enemy.SetAngleAwayFromTarget();
                                else
                                    enemy.SetAngleToTargetPlaceable();
                            }

                            if (Utility.Roll8())// clear target placeable occasionally
                            {
                                enemy.TargetPlaceable = null;
                            }
                        }

                        if (Utility.Roll8() && enemy.Target != null) {
                            var spawnedCount = (from e in (List<Enemy>)enemy.Bag.Spawned where !e.IsDead select e).Count();
                            if (spawnedCount <= 1) {
                                var flyerType = EnemyBase.Get("Drake");
                                Enemy drake;
                                drake = enemy.Map.AddEnemy(flyerType, enemy.Position + new Vector2(0, -100));
                                enemy.Bag.Spawned.Add(drake);

                                drake = enemy.Map.AddEnemy(flyerType, enemy.Position + new Vector2(0, -100));
                                enemy.Bag.Spawned.Add(drake);
                            }
                        }
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Attack1:
                            return age / 100 % 9;
                        case Animation.None:
                            return age / 100 % 9;
                        default:
                            return 0;
                    }
                },
                OnDie = (enemy) => {
                    //Skyrealm Loot
                    var LootLocation = enemy.Map.FindPlaceables("SlimeChest").FirstOrDefault();

                    if (LootLocation != null) {
                        var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), LootLocation.Position);
                        chest.AddItem(new Item() { TypeId = ItemId.Silver, Amount = 10 });
                        chest.AddItem(new Item() { TypeId = ItemId.LightOrb, Amount = 15 });
                        chest.AddItem(new Item() { TypeId = ItemId.Emerald, Amount = 9 });
                        chest.AddItem(new Item() { TypeId = ItemId.EnergyCell, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.EchoCrystal, Amount = 1 });
                        chest.AddItem(new Item() { TypeId = ItemId.ExtraLife, Amount = 5 });
                    }

                    //Water Loot
                    LootLocation = enemy.Map.FindPlaceables("WaterBossLoot").FirstOrDefault();

                    if (LootLocation != null) {
                        var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), LootLocation.Position);
                        chest.AddItem(new Item() { TypeId = ItemId.Darksteel, Amount = 10 });
                        chest.AddItem(new Item() { TypeId = ItemId.LightOrb, Amount = 15 });
                        chest.AddItem(new Item() { TypeId = ItemId.Sapphire, Amount = 9 });
                        chest.AddItem(new Item() { TypeId = ItemId.SuperEnergyCell, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.DarksteelSuperconductor, Amount = 1 });
                        chest.AddItem(new Item() { TypeId = ItemId.ExtraLife, Amount = 5 });
                        enemy.Map.TriggerSwitches("WaterBossDoor");
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 102,
                Name = "Sky Boss",
                SpriteWidth = 356,
                SpriteHeight = 258,
                AlternateTextureName = "Sky Boss Gun",
                ShootingOrigin = new Vector2(0, 78),
                ShootingOffset = 178,
                MaxHealth = 600,
                CollisionDamage = 100,
                CollisionKnockback = 1000,
                IsBoss = true,
                IsFlying = true,
                IsFlyingHeightFixed = true,
                Speed = 400,
                IsImmuneToKnockback = true,
                CanSeeThruWalls = true,
                BloodColor = Color.Black,
                OnDraw = (enemy, spriteBatch, position, frame) => {
                    spriteBatch.Draw(enemy.Type.AlternateTexture, position + new Vector2(0, 78), null, Color.White, enemy.Direction, new Vector2(enemy.Type.AlternateTexture.Width / 2, enemy.Type.AlternateTexture.Height / 2), 1, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                },
                OnThink = (enemy) => {
                    if (enemy.Target != null) {
                        enemy.WalkTowardTarget();
                        enemy.Direction = enemy.GetShootingAngle();
                    }
                },
                OnLongThink = (enemy) => {
                    if (enemy.Target == null) {
                        enemy.Health = enemy.MaxHealth;
                    } else {
                        enemy.Velocity = Vector2.Zero;
                        if (enemy.CooldownCheck(500))
                            enemy.ShootAtTarget(ProjectileId.DiggingAndDamaging);

                        if (Utility.Roll8()) {
                            var enemyType = EnemyBase.Get("Exploder");
                            enemy.Map.AddEnemy(enemyType, enemy.Position);
                        }
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return Math.Min(age / 100, 4);
                        default:
                            return 0;
                    }
                },
                OnRemove = (enemy) => {
                    enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position + Utility.RandomVector() * 200, Type = ParticleEffect.Explosion });
                    enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position + Utility.RandomVector() * 200, Type = ParticleEffect.Explosion });
                    enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position + Utility.RandomVector() * 200, Type = ParticleEffect.Explosion });
                    enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position + Utility.RandomVector() * 200, Type = ParticleEffect.Explosion });
                    enemy.Map.AddEntity(new ParticleEmitter() { Position = enemy.Position, Type = ParticleEffect.SkybossExplode, Color = enemy.Type.BloodColor, Value = 2 });

                    var portalPointOfIntrest = enemy.Map.FindPlaceables("skyPort").FirstOrDefault();
                    if (portalPointOfIntrest != null) {
                        var bossportal = enemy.Map.AddPlaceable(null, ItemBase.Get("Portal"), portalPointOfIntrest.Position);
                        bossportal.Value = "overworld";
                    }

                    //Reward Chest 
                    var LootLocation = enemy.Map.FindPlaceables("skyLoot").FirstOrDefault();

                    if (LootLocation != null) {
                        var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), LootLocation.Position);
                        chest.AddItem(new Item() { TypeId = ItemId.SilverSuperconductor, Amount = 1 });
                        chest.AddItem(new Item() { TypeId = ItemId.LightOrb, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.Silver, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.Sapphire, Amount = 5 });
                    }
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 103,
                Name = "Dragon",
                Description = "Boss for the Vesuvius Dungeon",
                SpriteWidth = 320,
                SpriteHeight = 320,
                MaxHealth = 900,
                CollisionDamage = 25,
                CollisionKnockback = 1000,
                IsBoss = true,
                IsFlying = true,
                IsFlyingHeightFixed = false,
                PenetrateWalls = false, // since this is set the health of this enemy will never clear
                Speed = 300,
                CanMoveWhileDead = true,
                IsImmuneToKnockback = true,
                CanSeeThruWalls = true,
                OnSpawn = (enemy, tier) => {
                    enemy.SetAnimation(Animation.Hovering);
                },
                OnLongThink = (enemy) => {
                    switch (enemy.Animation) {
                        case Animation.Hovering:
                            enemy.Speed = 300;

                            // clear target placeable occasionally
                            if (Utility.Roll8()) {
                                enemy.TargetPlaceable = null;
                            }

                            // go towards one of the pathing points of interest
                            if (enemy.TargetPlaceable == null) {
                                var targets = enemy.Map.FindPlaceables("Pathing").ToArray();
                                if (targets.Length > 0)
                                    enemy.TargetPlaceable = targets[Utility.Next(targets.Length)];
                            } else {
                                enemy.SetAngleToTargetPlaceable();
                            }

                            if (enemy.Target != null) {
                                if (enemy.CooldownCheck(500)) {
                                    // A large fireball that trails fire and does 25% damage.
                                    enemy.ShootAtTarget(ProjectileId.Fireball, Utility.RandomAngle(15));
                                }

                                if (Utility.Roll4()) {
                                    // this is the gun that penetrates walls
                                    enemy.ShootAtTarget(ProjectileId.LongRangeBlaster);
                                }

                                if (Utility.Roll16()) {
                                    enemy.SetAnimation(Animation.Attack1);
                                    enemy.SetAngleToTarget();
                                }
                            }
                            break;
                        case Animation.Attack1:
                            // charge attack that will break thru walls
                            if (enemy.IsTargetCloserThan(150))
                                enemy.SetAnimation(Animation.Idle1);

                            if (enemy.IsBlocked) {
                                enemy.Map.RenderBrush(enemy.FacePosition + new Vector2(0, -80), Brush.Size10, Material.Air, 7);
                                enemy.Map.RenderBrush(enemy.FacePosition + new Vector2(0, 80), Brush.Size10, Material.Air, 7);
                                enemy.Map.RenderBrush(enemy.FacePosition + new Vector2(0, -160), Brush.Size10, Material.Air, 7);
                                enemy.Map.RenderBrush(enemy.FacePosition + new Vector2(0, 160), Brush.Size10, Material.Air, 7);
                                enemy.Map.RenderBrush(enemy.FacePosition + new Vector2(0, 0), Brush.Size10, Material.Air, 7);
                                enemy.SetAnimation(Animation.Idle1);
                            }

                            if (enemy.IsOnGround || enemy.IsAgainstCeiling) {
                                enemy.Map.RenderBrush(enemy.FacePosition + new Vector2(80, 0), Brush.Size10, Material.Air, 7);
                                enemy.Map.RenderBrush(enemy.FacePosition + new Vector2(-80, 0), Brush.Size10, Material.Air, 7);
                                enemy.Map.RenderBrush(enemy.FacePosition + new Vector2(-160, 0), Brush.Size10, Material.Air, 7);
                                enemy.Map.RenderBrush(enemy.FacePosition + new Vector2(160, 0), Brush.Size10, Material.Air, 7);
                                enemy.Map.RenderBrush(enemy.FacePosition + new Vector2(0, 0), Brush.Size10, Material.Air, 7);
                                enemy.SetAnimation(Animation.Idle1);
                            } else {
                                if (enemy.AnimationAge < 1500) {
                                    enemy.Speed = 75;
                                    enemy.SetAngleAwayFromTarget();
                                }
                                if (enemy.AnimationAge > 1500) {
                                    enemy.Speed = ((enemy.AnimationAge - 600) / 100) * ((enemy.AnimationAge - 1500) / 100);
                                    enemy.SetAngleToTarget();
                                }
                            }
                            break;
                        case Animation.Idle1:
                            enemy.Speed = 75;
                            enemy.SetAngleAwayFromTarget();
                            if (enemy.AnimationAge > 1500)
                                enemy.SetAnimation(Animation.Hovering);
                            break;
                    }
                },
                OnDie = (enemy) => {
                    var chestPoI = enemy.Map.FindPlaceables("Death").FirstOrDefault();
                    if (chestPoI != null) {
                        var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), chestPoI.Position);
                        chest.AddItem(new Item() { TypeId = ItemId.Uranium, Amount = 10 });
                        chest.AddItem(new Item() { TypeId = ItemId.LightOrb, Amount = 15 });
                        chest.AddItem(new Item() { TypeId = ItemId.Diamond, Amount = 9 });
                        chest.AddItem(new Item() { TypeId = ItemId.FullEnergyCell, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.TurboSuperconductor, Amount = 1 });
                    }

                    var portalPointOfIntrest = enemy.Map.FindPlaceables("Portal").FirstOrDefault();
                    if (portalPointOfIntrest != null) {
                        var bossportal = enemy.Map.AddPlaceable(null, ItemBase.Get("Portal"), portalPointOfIntrest.Position);
                        bossportal.Value = "overworld";
                    }
                },

            });

            EnemyBase.AddItem(new EnemyType {
                Id = 104,
                Name = "Harpy",
                Description = "Grabs the player and drags him away.",
                SpriteWidth = 120,
                SpriteHeight = 180,
                MaxHealth = 350,
                CollisionDamage = 4,
                CollisionKnockback = 0,
                IsBoss = true,
                IsFlying = true,
                IsFlyingHeightFixed = false,
                //CanSeeThruWalls = true,
                CoolDown = 500,
                Speed = 450,
                OnSpawn = (enemy, tier) => {
                    enemy.SetAngleToTarget();
                    enemy.SetAnimation(Animation.Hovering);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                },
                OnThink = (enemy) => {
                    if (enemy.Target == null) {
                        enemy.SetAnimation(Animation.Hovering);
                        enemy.Health = enemy.MaxHealth;
                    }
                    if (enemy.AnimationAge > 6000 && enemy.Target != null) {
                        enemy.Speed = 500;
                        enemy.SetAnimation(Animation.Attack1);
                    }

                    switch (enemy.Animation) {
                        case Animation.Attack1:
                            if (enemy.IsBlocked || enemy.IsAgainstCeiling)
                                enemy.Direction = MathHelper.WrapAngle(enemy.Direction + (float)Math.PI / 2);
                            else {
                                enemy.SetAngleToTarget();
                                enemy.Velocity = Vector2.Zero;
                            }

                            //grab player
                            if (enemy.IsTargetCloserThan(98)) {
                                enemy.SetAnimation(Animation.LatchedOn);
                            }

                            if (enemy.AnimationAge > 4000)
                                enemy.SetAnimation(Animation.Hovering);
                            break;
                        case Animation.Hovering:
                            ///Hover over player
                            if (enemy.Target != null && !enemy.IsTargetCloserThan(600))
                                enemy.SetAngleToTarget();

                            if (enemy.IsBlocked || enemy.IsAgainstCeiling || enemy.IsOnGround)
                                enemy.Direction = MathHelper.WrapAngle(enemy.Direction + (float)Math.PI / 2);

                            if (enemy.Target != null && enemy.Position.Y >= (enemy.Target.Position.Y - 200)) {
                                var newDir = Utility.RandomAngle(180) + (float)(3 * Math.PI / 2);
                                enemy.Direction = newDir;
                            }

                            if (Utility.Roll8() && enemy.Target != null && enemy.IsCooledDown()) {
                                enemy.ShootAtTarget(ProjectileId.Spike);
                            }
                            break;
                        case Animation.LatchedOn:
                            if (enemy.Target == null || enemy.Target.IsDead) {
                                enemy.SetAnimation(Animation.Hovering);
                                enemy.Target = null;
                            } else {
                                enemy.Target.Position = enemy.Position + new Vector2(0, 90);
                                enemy.Target.Velocity = enemy.Velocity;
                                enemy.Direction = (float)(3 * (Math.PI / 2));//up
                                if (enemy.IsAgainstCeiling) {
                                    enemy.Speed = 450;
                                    enemy.SetAnimation(Animation.Hovering);
                                }
                            }
                            break;
                    }
                },
                OnDie = (enemy) => {
                    enemy.Map.TriggerSwitches("lava2");
                    enemy.Drop(ItemId.Ectoplasm, Utility.Next(50) + 10);

                    //Botanica loot

                    var LootPointOfIntrest = enemy.Map.FindPlaceables("harpyLoot").FirstOrDefault();
                    if (LootPointOfIntrest != null) {
                        var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), LootPointOfIntrest.Position);

                        chest.AddItem(new Item() { TypeId = ItemId.CopperOre, Amount = 15 });
                        chest.AddItem(new Item() { TypeId = ItemId.LightOrb, Amount = 10 });
                        chest.AddItem(new Item() { TypeId = ItemId.LesserEnergyCell, Amount = 5 });
                    }

                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 105,
                Name = "Basilisk",
                Description = "Stationary monster that shoots fire.",
                SpriteWidth = 280,
                SpriteHeight = 300,
                MaxHealth = 200,
                Defense = 3,
                CollisionDamage = 100,
                CollisionKnockback = 1000,
                CoolDown = 1000,
                IsBoss = true,
                Speed = 0,
                DeathAnimationLengthInSeconds = 10,
                OnSpawn = (enemy, tier) => {
                    enemy.Bag.Spawned = new List<Enemy>();
                },
                OnLongThink = (enemy) => {

                    var bossDoor = enemy.Map.FindPlaceables("mini2").FirstOrDefault();
                    if (enemy.Target == null || enemy.Target.IsDead) {
                        enemy.Health = enemy.MaxHealth;
                        if (bossDoor != null) {
                            bossDoor.IsSolid = false;
                        }
                    } else {
                        if (bossDoor != null)
                            bossDoor.IsSolid = true;

                        if (enemy.Target != null && enemy.IsTargetCloserThan(800)) {
                            enemy.LobObjectAtTarget(ProjectileId.Comet);
                        }

                        if (Utility.Roll8()) {
                            enemy.ShootAtAngle(ProjectileId.MiniFlame_2, (float)2.9, false);
                            enemy.ShootAtAngle(ProjectileId.MiniFlame_2, (float)3.5, false);
                        }
                    }
                },
                OnDie = (enemy) => {
                    //Botanica Version Loot
                    var lootLocation = enemy.Map.FindPlaceables("BotanicaLoot2").FirstOrDefault();
                    if (lootLocation != null) {
                        var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), lootLocation.Position);

                        chest.AddItem(new Item() { TypeId = ItemId.Topaz, Amount = 8 });
                        chest.AddItem(new Item() { TypeId = ItemId.GunnDiode, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.MercuryCell, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.LesserEnergyCell, Amount = 5 });
                    }
                    //Jungle Version of Loot
                    lootLocation = enemy.Map.FindPlaceables("JungleLoot1").FirstOrDefault();
                    if (lootLocation != null) {
                        var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), lootLocation.Position);

                        chest.AddItem(new Item() { TypeId = ItemId.Emerald, Amount = 8 });
                        chest.AddItem(new Item() { TypeId = ItemId.GunnDiode, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.MercuryCell, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.MajorEnergyCell, Amount = 5 });
                    }

                    enemy.Drop(ItemId.Ectoplasm, Utility.Next(50) + 10);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 106,
                Name = "Syra",
                Description = "A female faerie like monster that spawns sprout-lings on the ground either by dropping them or placing them. She spawns these at a steady pace and it is her main attack.",
                SpriteWidth = 120,
                SpriteHeight = 120,
                MaxHealth = 200,
                Defense = 40,
                CollisionDamage = 20,
                CollisionKnockback = 1000,
                IsBoss = true,
                IsFlying = true,
                Speed = 300,
                SoundName = "Harpy",
                OnSpawn = (enemy, tier) => {
                    enemy.SetAnimation(Animation.Hovering);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                    enemy.Bag.Spawned = new List<Enemy>();
                    enemy.Bag.Targets = enemy.Map.FindPlaceables("Pathing").ToArray();
                },
                OnLongThink = (enemy) => {
                    var bossDoor = enemy.Map.FindPlaceables("syraDoor").FirstOrDefault();

                    if ((float)enemy.Health / enemy.MaxHealth <= .25)
                        enemy.Speed = 450;
                    else if ((float)enemy.Health / enemy.MaxHealth <= .5)
                        enemy.Speed = 400;
                    else if ((float)enemy.Health / enemy.MaxHealth <= .75)
                        enemy.Speed = 350;

                    if (enemy.Target == null || enemy.Target.IsDead) {
                        if (bossDoor != null) {
                            bossDoor.IsSolid = false;
                        }
                        enemy.Health = enemy.MaxHealth;

                        var spawn = enemy.Map.FindPlaceables("syraSpawn").FirstOrDefault();
                        if (spawn != null) {
                            enemy.TargetPlaceable = spawn;
                        }

                        if (enemy.TargetPlaceable != null)
                            enemy.SetAngleToTargetPlaceable();

                        foreach (Enemy spawned in enemy.Bag.Spawned) {
                            spawned.Remove();
                        }


                        if (enemy.IsOnGround || enemy.IsAgainstCeiling || enemy.IsBlocked)
                            enemy.SetAngleAwayFromTarget();

                        enemy.Bag.Spawned.Clear();
                    } else {
                        if (bossDoor != null) {
                            bossDoor.IsSolid = true;
                        }

                        switch (enemy.Animation) {
                            case Animation.Hovering:
                                enemy.Speed = 300; {
                                    enemy.IsFlying = true;
                                    enemy.Speed = 300;
                                    enemy.Defense = 80;

                                    ///Hover over player
                                    if (!enemy.IsTargetCloserThan(550))
                                        enemy.SetAngleToTarget();

                                    if (enemy.IsTargetCloserThan(300))
                                        enemy.SetAngleToTarget();

                                    if (enemy.IsBlocked) {
                                        enemy.Direction = -enemy.Direction;
                                    }

                                    //pathing 
                                    if (enemy.AnimationAge >= 2000 || enemy.TargetPlaceable == null) // clear target placeable occasionally
                                    {
                                        enemy.SetAnimation(Animation.Idle1);
                                    } else {
                                        enemy.SetAngleToTargetPlaceable();
                                    }
                                    enemy.Velocity = Vector2.Zero;

                                    if (Utility.Roll16()) {
                                        var spawnedCount = (from e in (List<Enemy>)enemy.Bag.Spawned where !e.IsDead select e).Count();

                                        if (spawnedCount < 3) {
                                            var enemyType = EnemyBase.Get("Sprout");
                                            Enemy sprout;
                                            sprout = enemy.Map.AddEnemy(enemyType, enemy.Position);
                                            enemy.Bag.Spawned.Add(sprout);
                                        }
                                    }

                                    if (Utility.Roll4())
                                        enemy.ShootAtTarget(ProjectileId.Spike);
                                    break;
                                }
                            case Animation.Attack1: {
                                    enemy.IsFlying = false;
                                    enemy.Speed = 0;
                                    enemy.Defense = 0;

                                    var spawnedCount = (from e in (List<Enemy>)enemy.Bag.Spawned where !e.IsDead select e).Count();

                                    if (spawnedCount < 3) {
                                        var enemyType = EnemyBase.Get("Valee");
                                        Enemy valee;
                                        valee = enemy.Map.AddEnemy(enemyType, enemy.Position + new Vector2(0, -60));
                                        enemy.Bag.Spawned.Add(valee);
                                    }
                                    if (enemy.AnimationAge >= 8000)
                                        enemy.SetAnimation(Animation.Hovering);
                                    break;
                                }
                            case Animation.Idle1: {
                                    //enemy.Speed = 0;
                                    if (enemy.Bag.Targets.Length > 0)
                                        enemy.TargetPlaceable = enemy.Bag.Targets[Utility.Next(enemy.Bag.Targets.Length)];

                                    //drop on player
                                    if (Math.Abs(enemy.Position.X - enemy.Target.Position.X) <= 150.0 || Utility.Roll16())
                                        enemy.SetAnimation(Animation.Attack1);
                                    else
                                        enemy.SetAnimation(Animation.Hovering);
                                    break;
                                }
                        }
                    }
                },
                OnDie = (enemy) => {
                    enemy.Map.TriggerSwitches("lava5");

                    var portal = enemy.Map.FindPlaceables("exit").FirstOrDefault();
                    if (portal != null) {
                        var exit = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.Portal), portal.Position);
                        exit.Value = "overworld";
                    }
                    enemy.Drop(ItemId.Ectoplasm, Utility.Next(15));

                    //Botanica's loot
                    var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), enemy.Position);
                    if (chest != null) {
                        chest.AddItem(new Item() { TypeId = ItemId.LightOrb, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.LesserEnergyCell, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.Detector, Amount = 3 });
                        chest.AddItem(new Item() { TypeId = ItemId.MercuryArcRectifier, Amount = 3 });
                        chest.AddItem(new Item() { TypeId = ItemId.EctoplasmCore, Amount = 3 });
                    }

                    //Portal Loot
                    var chestLocation = enemy.Map.FindPlaceables("PortalLoot").FirstOrDefault();
                    if (chestLocation != null) {
                        chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), chestLocation.Position);
                    }
                    if (chest != null) {
                        chest.AddItem(new Item() { TypeId = ItemId.LightOrb, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.HalfEnergyCell, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.Detector, Amount = 3 });
                        chest.AddItem(new Item() { TypeId = ItemId.MercuryArcRectifier, Amount = 3 });
                        chest.AddItem(new Item() { TypeId = ItemId.EctoplasmCore, Amount = 3 });
                        chest.AddItem(new Item() { TypeId = ItemId.AdamantiumSuperconductor, Amount = 1 });
                    }

                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 108,
                Name = "Jungle Boss",
                Description = "Acid boss that follows a path and shoots acid spit that explodes acid.",
                SpriteWidth = 192,
                SpriteHeight = 192,
                MaxHealth = 400,
                Speed = 150,
                CollisionDamage = 20,
                CollisionKnockback = 500,
                IsFlying = true,
                IsBoss = true,
                PenetrateWalls = true,
                CanSeeThruWalls = true,
                OnSpawn = (enemy, tier) => {
                    enemy.Bag.CurrentTarget = 0;
                    enemy.Bag.Targets = new List<Placeable>();
                    enemy.SetRandomAngle();
                    enemy.CheckFacingDirection();
                    enemy.SetAnimation(Animation.Attack1);
                    enemy.AnimationStart += TimeSpan.FromSeconds(Utility.NextDouble());
                    foreach (var placable in enemy.Map.FindPlaceables("BossSpawn")) {
                        placable.Flag = false;
                    }
                },
                OnLongThink = (enemy) => {
                    if (enemy.TargetPlaceable == null) {
                        enemy.Bag.Targets = enemy.Map.FindPlaceables("Pathing").ToArray();
                        if(enemy.Bag.Targets != null)
                            enemy.TargetPlaceable = enemy.Bag.Targets[enemy.Bag.CurrentTarget++];
                        if (enemy.TargetPlaceable != null)
                            enemy.SetAngleToTargetPlaceable();
                    }

                    if (enemy.TargetPlaceable != null && enemy.IsTargetPlaceableCloserThan(100)) {
                        if (enemy.Bag.CurrentTarget == enemy.Bag.Targets.Length)
                            enemy.Bag.CurrentTarget = 0;
                        var currentTarget = enemy.Bag.CurrentTarget;
                        enemy.TargetPlaceable = enemy.Bag.Targets[enemy.Bag.CurrentTarget++];
                        enemy.SetAngleToTargetPlaceable();
                    }

                    enemy.CheckFacingDirection();

                    enemy.Velocity = Vector2.Zero;

                    if (enemy.Target != null && enemy.AnimationAge > 6000 && enemy.Animation != Animation.Attack1)
                        enemy.SetAnimation(Animation.Attack1);

                    if (enemy.Animation == Animation.Attack1) {
                        foreach (var placable in enemy.Map.FindPlaceables("BossSpawn")) {
                            placable.Flag = false;
                        }
                        if (enemy.Target != null && enemy.CooldownCheck(900)) {
                                enemy.ShootAtTarget(ProjectileId.ExplosiveAcidSpit);
                        }

                        if (enemy.AnimationAge >= 6000)
                            enemy.SetAnimation(Animation.None);
                    }

                    if (enemy.Animation == Animation.None) {
                        foreach (var placable in enemy.Map.FindPlaceables("BossSpawn")) {
                            placable.Flag = true;
                        }
                        //if (enemy.Target != null) {
                        //    enemy.SetAngleToTarget();
                        //    enemy.CheckFacingDirection();
                        //}
                    }
                },
                OnDie = (enemy) => {
                    foreach (var placable in enemy.Map.FindPlaceables("BossSpawn"))
                    {
                        placable.Flag = false;
                    }

                    enemy.Map.TriggerSwitches("BossDoor");

                    var LootLocation = enemy.Map.FindPlaceables("FireChest").FirstOrDefault();

                    if (LootLocation != null) {
                        var chest = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.RewardHypercube), LootLocation.Position);
                        chest.AddItem(new Item() { TypeId = ItemId.Gold, Amount = 10 });
                        chest.AddItem(new Item() { TypeId = ItemId.LightOrb, Amount = 15 });
                        chest.AddItem(new Item() { TypeId = ItemId.Emerald, Amount = 9 });
                        chest.AddItem(new Item() { TypeId = ItemId.EnergyCell, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.EchoCrystal, Amount = 5 });
                        chest.AddItem(new Item() { TypeId = ItemId.GoldSuperconductor, Amount = 1 });
                    }

                },
            });


            EnemyBase.AddItem(new EnemyType {
                Id = 200,
                Name = "Final Boss Part 1",
                Description = "Phase 1 of the final boss",
                SpriteWidth = 1024,
                SpriteHeight = 1024,
                MaxHealth = 300,
                Defense = 1,
                CollisionDamage = 100,
                CollisionKnockback = 1000,
                IsBoss = true,
                CanSeeThruWalls = true,
                DeathAnimationLengthInSeconds = 5000,
                IsImmuneToKnockback = true,
                Speed = 0,
                // ShootingOrigin = new Vector2(512, 256),
                OnSpawn = (enemy, tier) => {
                    enemy.Bag.nearDeath = false;
                    enemy.Bag.allreadyExploded = false;
                },
                OnLongThink = (enemy) => {
                    if (enemy.Target == null || enemy.Target.IsDead) {
                        enemy.Health = enemy.MaxHealth;
                    } else {
                        //Random Volcano explosions
                        string name = "eruption" + string.Format("{0}", Utility.Next(3) + 1);
                        var eruption = enemy.Map.FindPlaceables(name).FirstOrDefault();

                        if (eruption != null) {
                            if (Utility.Roll4()) {
                                enemy.Map.TriggerSwitches(name);
                            }
                        }

                        if (enemy.IsTargetCloserThan(1400)) {
                            enemy.SetAngleToTarget();
                            enemy.Velocity = Vector2.Zero;
                            if (Utility.Roll8())
                                enemy.ShootAtTarget(ProjectileId.Fireball_2);
                            else if (Utility.Roll4())
                                enemy.ShootAtTarget(ProjectileId.MiniFlame);
                        }
                        if ((float)enemy.Health / enemy.MaxHealth <= .25)
                            enemy.Bag.nearDeath = true;
                        if (enemy.Bag.nearDeath && !enemy.Bag.allreadyExploded) {
                            enemy.Bag.allreadyExploded = true;
                            enemy.Target.Map.LockTier = 9;
                            foreach (var wall in enemy.Map.FindPlaceables("neardeath")) {
                                enemy.Map.Explode(wall.Position, 200, 8);
                                enemy.Map.Explode(wall.Position, 200, 8);
                                enemy.Map.Explode(wall.Position, 200, 8);
                            }
                            enemy.Delay(3000, () => {
                                enemy.Target.Map.LockTier = 8;
                            });
                        }
                    }
                },

                OnDie = (enemy) => {
                    enemy.Target.Map.LockTier = 9;
                    enemy.Target.MessageToClient("Digging is allowed", MessageType.Chat);
                    enemy.Delay(8000, () => {
                        enemy.Map.Explode(enemy.Position, 1000, 0);
                        enemy.Map.AddEnemy(EnemyBase.Get(201), enemy.Position);
                        foreach (var wall in enemy.Map.FindPlaceables("wallexplosion")) {
                            enemy.Map.Explode(wall.Position, 200, 8);
                            enemy.Map.Explode(wall.Position, 200, 8);
                            enemy.Map.Explode(wall.Position, 200, 8);
                        }
                    });
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 201,
                Name = "Final Boss Part 2",
                Description = "Phase 2, Follows player destroying the landscape as he moves",
                SpriteWidth = 512,
                SpriteHeight = 512,
                MaxHealth = 10,
                Defense = Int16.MaxValue,
                CollisionDamage = 50,
                CollisionKnockback = 1000,
                IsBoss = true,
                IsFlying = true,
                PenetrateWalls = true,
                CanSeeThruWalls = true,
                IsImmuneToKnockback = true,
                Speed = 100,
                OnSpawn = (enemy, tier) => {
                    enemy.Bag.Explode = true;
                },
                OnLongThink = (enemy) => {
                    if (enemy.Target == null || enemy.Target.IsDead) {
                        enemy.Health = enemy.MaxHealth;
                    } else {
                        //To keep up with the player so he doesnt despawn
                        if (!enemy.IsTargetCloserThan(1800))
                            enemy.Speed = 400;
                        if (enemy.IsTargetCloserThan(1800))
                            enemy.Speed = 120;
                        if (enemy.IsTargetCloserThan(1000))
                            enemy.Speed = 60;

                        if (enemy.Bag.Explode) {
                            if (enemy.Map.GetMaterialAtPixel(enemy.FacePosition) != Material.Air) {
                                enemy.ShootAtTarget(ProjectileId.Grenade, 0);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 15);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 30);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 45);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 60);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 75);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 90);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 105);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 120);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 135);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 150);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 165);
                                enemy.ShootAtTarget(ProjectileId.Grenade, 180);

                                ////left
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X - 512, enemy.Position.Y), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X - 512, enemy.Position.Y - 412), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X - 512, enemy.Position.Y - 512), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X - 512, enemy.Position.Y - 612), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X - 412, enemy.Position.Y - 612), Brush.Mega, Material.NaturalGas, 9);

                                ////top
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X - 412, enemy.Position.Y - 612), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X - 212, enemy.Position.Y - 612), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X + 212, enemy.Position.Y - 612), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X + 412, enemy.Position.Y - 612), Brush.Mega, Material.NaturalGas, 9);

                                ////right
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X + 412, enemy.Position.Y - 612), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X + 512, enemy.Position.Y - 612), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X + 512, enemy.Position.Y - 512), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X + 512, enemy.Position.Y - 412), Brush.Mega, Material.NaturalGas, 9);
                                //enemy.Map.RenderBrush(new Vector2(enemy.Position.X + 512, enemy.Position.Y), Brush.Mega, Material.NaturalGas, 9);
                            }
                            enemy.Bag.Explode = false;
                        }

                        if (Utility.Roll4()) {
                            if (enemy.Map.GetMaterialAtPixel(enemy.FacePosition) != Material.Air)
                                enemy.Map.RenderBrush(enemy.FacePosition, Brush.Size10, Material.NaturalGas, 9);
                        }

                        enemy.SetAngleToTarget();
                        if (Utility.Roll4())
                            enemy.ShootAtTarget(ProjectileId.Grenade);

                        if (Utility.Roll32())
                            enemy.Bag.Explode = true;

                        //enemy.ShootAtTarget(ProjectileId.DiggingAndDamaging, Utility.RandomAngle(45));
                        //enemy.ShootAtTarget(ProjectileId.DiggingAndDamaging, Utility.RandomAngle(45));
                        //enemy.ShootAtTarget(ProjectileId.DiggingAndDamaging, Utility.RandomAngle(45));
                    }
                },
                OnDie = (enemy) => {
                    //                    enemy.Map.AddEnemy(EnemyBase.Get(201), enemy.Position);
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 202,
                Name = "Final Boss Part 3",
                Description = "Phase 3, Attack Player in his personal room",
                SpriteWidth = 512,
                SpriteHeight = 512,
                MaxHealth = 1000,
                Defense = 100,
                CollisionDamage = 50,
                CollisionKnockback = 1000,
                IsBoss = true,
                IsFlying = true,
                PenetrateWalls = true,
                CanSeeThruWalls = true,
                IsImmuneToKnockback = true,
                Speed = 175,
                OnSpawn = (enemy, tier) => {
                    enemy.SetAnimation(Animation.Hovering);
                    enemy.Bag.Spawned = new List<Enemy>();
                    enemy.Bag.Ground = new List<Enemy>();
                    enemy.Bag.Mini = new List<Enemy>();
                    enemy.Bag.Dead = new List<Enemy>();
                },
                OnLongThink = (enemy) => {
                    var spawnedCount = (from e in (List<Enemy>)enemy.Bag.Spawned where !e.IsDead select e).Count();
                    var spawnedCountGround = (from e in (List<Enemy>)enemy.Bag.Ground where !e.IsDead select e).Count();
                    var deathCounth = (from e in (List<Enemy>)enemy.Bag.Ground where e.IsDead select e).Count() + (from e in (List<Enemy>)enemy.Bag.Spawned where e.IsDead select e).Count();
                    var miniCount = (from e in (List<Enemy>)enemy.Bag.Mini where !e.IsDead select e).Count();

                    if (enemy.Health <= enemy.MaxHealth / 2 && enemy.Health >= enemy.MaxHealth / 4)
                        enemy.SetAnimation(Animation.Idle1);

                    if (enemy.Target == null) {
                        enemy.Health = enemy.MaxHealth;
                        enemy.SetAnimation(Animation.Hovering);
                        enemy.Target = null;

                        foreach (Enemy spawned in enemy.Bag.Spawned) {
                            spawned.Remove();
                        }
                        foreach (Enemy spawned in enemy.Bag.Ground) {
                            spawned.Remove();
                        }
                        foreach (Enemy spawned in enemy.Bag.Mini) {
                            spawned.Remove();
                        }
                        foreach (Enemy spawned in enemy.Bag.Dead) {
                            spawned.Remove();
                        }
                    }

                    switch (enemy.Animation) {
                        case Animation.Hovering: //His standard attacks pre-enrage
                            enemy.Defense = 100;
                            enemy.Speed = 175;

                            if (Utility.Roll8()) // clear target placeable occasionally
                            {
                                enemy.TargetPlaceable = null;
                            }

                            if (enemy.TargetPlaceable == null) {
                                var targets = enemy.Map.FindPlaceables("Pathing").ToArray();
                                if (targets.Length > 0)
                                    enemy.TargetPlaceable = targets[Utility.Next(targets.Length)];
                            } else {
                                enemy.SetAngleToTargetPlaceable();
                            }

                            if (Utility.Roll4())
                                enemy.ShootAtTarget(ProjectileId.AcidSpit);
                            else if (Utility.Roll4())
                                enemy.ShootAtTarget(ProjectileId.Fireball_2);

                            if (Utility.Roll16() && enemy.AnimationAge > 10000)
                                enemy.SetAnimation(Animation.Attack1);
                            break;
                        case Animation.Enrage: //When hp reaches 25% he besereks
                            enemy.Defense = 0;
                            enemy.Speed = 200;

                            if (Utility.Roll8()) // clear target placeable occasionally
                            {
                                enemy.TargetPlaceable = null;
                            }

                            if (enemy.TargetPlaceable == null) {
                                var targets = enemy.Map.FindPlaceables("Pathing").ToArray();
                                if (targets.Length > 0)
                                    enemy.TargetPlaceable = targets[Utility.Next(targets.Length)];
                            } else {
                                enemy.SetAngleToTargetPlaceable();
                            }

                            if (Utility.Roll4())
                                enemy.ShootAtTarget(ProjectileId.Fireball_2);

                            if (Utility.Roll4())
                                enemy.ShootAtAngle(ProjectileId.FlameThrower, (float)(Math.PI / 2), false);//down
                            else if (Utility.Roll4())
                                enemy.ShootAtAngle(ProjectileId.FlameThrower, (float)(Math.PI / 4), false);//right down
                            else if (Utility.Roll4())
                                enemy.ShootAtAngle(ProjectileId.FlameThrower, (float)(3 * (Math.PI / 4)), false);//left down

                            if (Utility.Roll8()) {
                                enemy.ShootAtAngle(ProjectileId.MiniFlame_2, 0, false);//right
                                enemy.ShootAtAngle(ProjectileId.MiniFlame_2, (float)(Math.PI / 4), false);//right down
                                enemy.ShootAtAngle(ProjectileId.MiniFlame_2, (float)(Math.PI / 2), false);//down
                                enemy.ShootAtAngle(ProjectileId.MiniFlame_2, (float)(3 * (Math.PI / 4)), false);//left down
                                enemy.ShootAtAngle(ProjectileId.MiniFlame_2, (float)Math.PI, false);//left
                            }
                            break;
                        case Animation.Attack1: //Becomes shielded and spawns adds and waits for the player to kill the adds
                            enemy.Defense = 2500;
                            enemy.Speed = 0;

                            var drakeType = EnemyBase.Get("Flame Drake");
                            enemy.Bag.Spawned.Add(enemy.Map.AddEnemy(drakeType, enemy.Position + new Vector2(160, -160)));
                            enemy.Bag.Spawned.Add(enemy.Map.AddEnemy(drakeType, enemy.Position + new Vector2(-160, 160)));
                            enemy.Bag.Spawned.Add(enemy.Map.AddEnemy(drakeType, enemy.Position + new Vector2(-160, -160)));
                            enemy.Bag.Spawned.Add(enemy.Map.AddEnemy(drakeType, enemy.Position + new Vector2(160, 160)));
                            enemy.SetAnimation(Animation.Attack2);
                            break;
                        case Animation.Attack2: //Randomly shoot out AoE
                            if (Utility.Roll8()) {
                                enemy.ShootAtAngle(ProjectileId.MiniFlame_2, 0, false);//right
                                enemy.ShootAtAngle(ProjectileId.MiniFlame_2, (float)(Math.PI / 4), false);//right down
                                enemy.ShootAtAngle(ProjectileId.MiniFlame_2, (float)(Math.PI / 2), false);//down
                                enemy.ShootAtAngle(ProjectileId.MiniFlame_2, (float)(3 * (Math.PI / 4)), false);//left down
                                enemy.ShootAtAngle(ProjectileId.MiniFlame_2, (float)Math.PI, false);//left
                            }

                            if (spawnedCount == 0)
                                enemy.SetAnimation(Animation.Hovering);
                            break;
                        case Animation.Idle1: //When he reaches 50% HP and he will hide and spawn adds to attack player, he loses HP over time and is shielding himself, enrages after the phase is over
                            var target = enemy.Map.FindPlaceables("Intermission").FirstOrDefault();

                            if (target != null) {
                                enemy.TargetPlaceable = target;
                                enemy.SetAngleToTargetPlaceable();
                            } else enemy.SetAnimation(Animation.Hovering);

                            enemy.Defense = 2000;

                            if (enemy.TargetPlaceable.WithinRange(enemy, 100)) {
                                enemy.Speed = 0;
                            }

                            if (enemy.AnimationAge > 4000) {
                                if (spawnedCount <= 1) {
                                    enemy.Health -= 1300;

                                    var drakeType2 = EnemyBase.Get("Flame Drake");
                                    enemy.Bag.Spawned.Add(enemy.Map.AddEnemy(drakeType2, enemy.Position + new Vector2(160, -160)));
                                    enemy.Bag.Spawned.Add(enemy.Map.AddEnemy(drakeType2, enemy.Position + new Vector2(-160, 160)));
                                    enemy.Bag.Spawned.Add(enemy.Map.AddEnemy(drakeType2, enemy.Position + new Vector2(-160, -160)));
                                    enemy.Bag.Spawned.Add(enemy.Map.AddEnemy(drakeType2, enemy.Position + new Vector2(160, 160)));
                                }

                                if (spawnedCountGround <= 1) {
                                    enemy.Health -= 1200;
                                    var crawlerType = EnemyBase.Get("Ground Trooper");
                                    Enemy crawler;
                                    crawler = enemy.Map.AddEnemy(crawlerType, enemy.Position + new Vector2(-200, 80));
                                    crawler.Velocity += new Vector2(-300, 500);
                                    crawler.IsFacingLeft = false;
                                    enemy.Bag.Ground.Add(crawler);

                                    crawler = enemy.Map.AddEnemy(crawlerType, enemy.Position + new Vector2(-200, 80));
                                    crawler.Velocity += new Vector2(-500, 500);
                                    crawler.IsFacingLeft = false;
                                    enemy.Bag.Ground.Add(crawler);

                                    crawler = enemy.Map.AddEnemy(crawlerType, enemy.Position + new Vector2(-200, 80));
                                    crawler.Velocity += new Vector2(-700, 500);
                                    crawler.IsFacingLeft = false;
                                    enemy.Bag.Ground.Add(crawler);

                                    crawler = enemy.Map.AddEnemy(crawlerType, enemy.Position + new Vector2(-200, 80));
                                    crawler.Velocity += new Vector2(-700, 500);
                                    crawler.IsFacingLeft = false;
                                    enemy.Bag.Ground.Add(crawler);
                                }

                                if (deathCounth % 10 == 0 && miniCount == 0) {
                                    var crawlerType = EnemyBase.Get("Brunnen");
                                    Enemy brun;
                                    brun = enemy.Map.AddEnemy(crawlerType, enemy.Position + new Vector2(-200, 80));
                                    brun.Velocity += new Vector2(-700, 500);
                                    brun.IsFacingLeft = false;
                                    enemy.Bag.Ground.Add(brun);
                                    enemy.Bag.Mini.Add(brun);
                                }
                            }

                            if (enemy.Health <= enemy.MaxHealth / 4)
                                enemy.SetAnimation(Animation.Enrage);

                            break;
                    }
                },
                OnDie = (enemy) => //Loot in victoryroom
                {
                    var portal = enemy.Map.AddPlaceable(null, ItemBase.Get(ItemId.Portal), enemy.Position);
                    portal.Value = "Victory";
                },
            });

            EnemyBase.AddItem(new EnemyType {
                Id = 210,
                Name = "Survivor",
                SpriteWidth = 64,
                SpriteHeight = 64,
                SpriteFramesPerRow = 8,
                MaxHealth = 1,
                Defense = Int32.MaxValue,
                Speed = 25,
                AvoidsCliffs = true,
                OnSpawn = (enemy, tier) => {
                    enemy.IsFacingLeft = Utility.Flip();
                    enemy.SetAnimation(Animation.Move);
                },
                OnLongThink = (enemy) => {
                    if (enemy.IsBlocked) {
                        enemy.IsFacingLeft = !enemy.IsFacingLeft;
                    }

                    if (enemy.Target != null) {
                        enemy.WalkTowardTarget();
                        enemy.Speed = 50;
                    } else {
                        enemy.Speed = 25;

                        if (Utility.Roll4())
                            enemy.Speed = 0;
                        if (Utility.Roll4())
                            enemy.Speed = 50;
                        if (Utility.Roll4())
                            enemy.IsFacingLeft = !enemy.IsFacingLeft;

                        if (enemy.IsTargetCloserThan(30) && !enemy.Target.IsGameCompleted) {
                            enemy.Target.IsGameCompleted = true;
                            enemy.Target.GiveItem(ItemId.TheKeysOfTheKingdom);
                        }
                    }
                },
                GetFrame = (animation, age) => {
                    switch (animation) {
                        case Animation.Move:
                            return age / 100 % 8;
                        default:
                            return 0;
                    }
                },
            });
        }

        static ItemId MonsterLoot()
        {
            if (Utility.Flip()) {
                switch (Utility.Next(9)) {
                    case 0:
                        return ItemId.GunnDiode;
                    case 1:
                        return ItemId.FieldEffectTransistor;
                    case 2:
                        return ItemId.GreenCrystal;
                    case 3:
                        return ItemId.BlueCrystal;
                    case 4:
                        return ItemId.GreenCrystal;
                    case 5:
                        return ItemId.MercuryCell;
                    case 6:
                        return ItemId.Thyratron;
                    case 7:
                        return ItemId.Thermistor;
                    case 8:
                        return ItemId.Wiring;
                }
            }

            return ItemId.Energy;
        }

        static ItemId MechanicalLoot(int tier)
        {
            if (tier == 1)
                return ItemId.Iron;
            if (tier == 2)
                return ItemId.Steel;
            if (tier == 3)
                return ItemId.Copper;
            if (tier == 4)
                return ItemId.Silver;
            if (tier == 5)
                return ItemId.Gold;
            if (tier == 6)
                return ItemId.Adamantium;
            if (tier == 8)
                return ItemId.Obsidian;
            else
                return ItemId.Einsteinium;
        }
    }
}