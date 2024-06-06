{
                progress = 0;
                Color[] pixels = new Color[inputTexture.width * inputTexture.height];
                // sobel filter
                Texture2D texNormal = new Texture2D(inputTexture.width, inputTexture.height, TextureFormat.RGB24, false, false);
                Vector3 vScale = new Vector3(0.3333f, 0.3333f, 0.3333f);
                for (int y = 0; y < inputTexture.height; y++)
                {
                    for (int x = 0; x < inputTexture.width; x++)
                    {
                        Color tc = texSource.GetPixel(x - 1, y - 1);
                        Vector3 cSampleNegXNegY = new Vector3(tc.r, tc.g, tc.g);
                        tc = texSource.GetPixel(x, y - 1);
                        Vector3 cSampleZerXNegY = new Vector3(tc.r, tc.g, tc.g);
                        tc = texSource.GetPixel(x + 1, y - 1);
                        Vector3 cSamplePosXNegY = new Vector3(tc.r, tc.g, tc.g);
                        tc = texSource.GetPixel(x - 1, y);
                        Vector3 cSampleNegXZerY = new Vector3(tc.r, tc.g, tc.g);
                        tc = texSource.GetPixel(x + 1, y);
                        Vector3 cSamplePosXZerY = new Vector3(tc.r, tc.g, tc.g);
                        tc = texSource.GetPixel(x - 1, y + 1);
                        Vector3 cSampleNegXPosY = new Vector3(tc.r, tc.g, tc.g);
                        tc = texSource.GetPixel(x, y + 1);
                        Vector3 cSampleZerXPosY = new Vector3(tc.r, tc.g, tc.g);
                        tc = texSource.GetPixel(x + 1, y + 1);
                        Vector3 cSamplePosXPosY = new Vector3(tc.r, tc.g, tc.g);
                        float fSampleNegXNegY = Vector3.Dot(cSampleNegXNegY, vScale);
                        float fSampleZerXNegY = Vector3.Dot(cSampleZerXNegY, vScale);
                        float fSamplePosXNegY = Vector3.Dot(cSamplePosXNegY, vScale);
                        float fSampleNegXZerY = Vector3.Dot(cSampleNegXZerY, vScale);
                        float fSamplePosXZerY = Vector3.Dot(cSamplePosXZerY, vScale);
                        float fSampleNegXPosY = Vector3.Dot(cSampleNegXPosY, vScale);
                        float fSampleZerXPosY = Vector3.Dot(cSampleZerXPosY, vScale);
                        float fSamplePosXPosY = Vector3.Dot(cSamplePosXPosY, vScale);
                        float edgeX = (fSampleNegXNegY - fSamplePosXNegY) * 0.25f + (fSampleNegXZerY - fSamplePosXZerY) * 0.5f + (fSampleNegXPosY - fSamplePosXPosY) * 0.25f;
                        float edgeY = (fSampleNegXNegY - fSampleNegXPosY) * 0.25f + (fSampleZerXNegY - fSampleZerXPosY) * 0.5f + (fSamplePosXNegY - fSamplePosXPosY) * 0.25f;
                        Vector2 vEdge = new Vector2(edgeX, edgeY) * normalStrength;
                        Vector3 norm = new Vector3(vEdge.x, vEdge.y, 1.0f).normalized;
                        Color c = new Color(norm.x * 0.5f + 0.5f, norm.y * 0.5f + 0.5f, norm.z * 0.5f + 0.5f, 1);
                        pixels[x + y * inputTexture.width] = c;
                    }
                    // progress bar
                    progress += progressStep;
                    if (EditorUtility.DisplayCancelableProgressBar(appName, "Creating normal map..", progress))
                    {
                        Debug.Log(appName + ": Normal map creation cancelled by user (strange texture results will occur)");
                        EditorUtility.ClearProgressBar();
                        break;
                    }
                }
