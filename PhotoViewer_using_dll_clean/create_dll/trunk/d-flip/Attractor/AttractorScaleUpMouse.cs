﻿using System;
using System.Collections.Generic;
//using System.Text;
using Microsoft.Xna.Framework;
using dflip.Element;
using dflip;
using dflip.Supplement;
using dflip.Manager;
using PhotoInfo;

namespace Attractor
{
    class AttractorScaleUpMouse : IAttractorSelection
    {
        private readonly Random rand = new Random();
        private readonly RandomBoxMuller randbm = new RandomBoxMuller();
        private int weight_ = 50;
        // added by Gengdai
        private float realMinScale = 0.0f;
        private float realMaxScale = 0.0f;

        public void select(Dock dock, ScrollBar sBar, AttractorWeight weight, List<Photo> photos, List<Photo> activePhotos, List<Stroke> strokes, SystemState systemState)
        {
            float MinPhotoSize = SystemParameter.MinPhotoScale(SystemParameter.ClientWidth, SystemParameter.ClientHeight, ResourceManager.MAXX, ResourceManager.MAXY, photos.Count) * 5;
            float MaxPhotoSize = SystemParameter.MaxPhotoScale(SystemParameter.ClientWidth, SystemParameter.ClientHeight, ResourceManager.MAXX, ResourceManager.MAXY, photos.Count) * 2;

            weight_ = weight.ScaleUpMouseWeight;
            // アトラクター選択
            foreach (Photo a in activePhotos)
            {
                float ds = 0; // スケール

                // added by Gengdai
                realMinScale = a.GetTexture().Width > a.GetTexture().Height ? MinPhotoSize * ResourceManager.MAXX / a.GetTexture().Width : MinPhotoSize * ResourceManager.MAXY / a.GetTexture().Height;
                realMaxScale = a.GetTexture().Width > a.GetTexture().Height ? MaxPhotoSize * ResourceManager.MAXX / a.GetTexture().Width : MaxPhotoSize * ResourceManager.MAXY / a.GetTexture().Height;


                // マウスに重なっているほど大きくしたい
                //if (a.IsGazeds)
                {
                    ds += (realMaxScale - a.Scale) * weight_ * 0.05f;
                    // サイズが最小値以下もしくは最大値以上になるのを防ぐ制約
                    if (a.Scale < realMinScale)
                    {
                        ds += (realMinScale - a.Scale) * weight_ * 0.01f;
                    }
                    else if (a.Scale > realMaxScale)
                    {
                        ds -= (a.Scale - realMaxScale) * weight_ * 0.01f;
                    }
                    // ノイズを付加する
                    if (false)
                    {
                        float variance = weight.NoiseWeight * 0.2f;
                        float noise = (float)randbm.NextDouble(variance);
                        ds += noise;
                    }
                }

                a.AddScale(ds);
            }
        }
    }
}
