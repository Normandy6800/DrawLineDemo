using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDraw {

    int width { get; }

    int height { get; }

    void Draw(Texture2D canvas, Color bgCor, Vector2 pos);
}
