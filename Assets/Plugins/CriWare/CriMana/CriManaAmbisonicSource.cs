﻿using UnityEngine;
using System.Collections;

/**
 * \addtogroup CRIMANA_UNITY_COMPONENT
 * @{
 */


/**
 * <summary>Ambisonic 音声つきムービの音源を制御するためのコンポーネントです。</summary>
 * \par 説明:
 * Ambisonic 音声つきムービの音源を制御するためのコンポーネントです。
 * CriManaMovieMaterial の  Advanced Audio モードを有効化することで、本コンポーネントがアタッチされた Audio Source オブジェクトが作成されます。
 * 本コンポーネントは音源の方向の更新処理を行います。
 * 本コンポーネントがアタッチされた GameObject の角度 (transform.eulerAngles ) を元に、音源の方向ベクトルを回転します。
 */
public class CriManaAmbisonicSource : MonoBehaviour
{
    #region Internal Variables
    private CriAtomEx3dSource atomEx3DsourceForAmbisonics;
    private Vector3 ambisonicSourceOrientationFront;
    private Vector3 ambisonicSourceOrientationTop;
    private Vector3 lastEulerOfAmbisonicSource;
    #endregion


    #region Public Method
    void Update()
    {
        UpdateAmbisonicSourceOrientation();
    }

    void OnEnable()
    {
        /* Ambisonic 音源の位置と向きを初期化 */
        atomEx3DsourceForAmbisonics = this.gameObject.transform.parent.GetComponent<CriManaMovieMaterial>().player.atomEx3DsourceForAmbisonics;
        if (atomEx3DsourceForAmbisonics == null)
        {
            Debug.LogError("atomEx3DsourceForAmbisonics == null");
            return;
        }
        ForceUpdateAmbisonicSourceOrientation();
    }
    #endregion


    #region Private Methods
    private void ForceUpdateAmbisonicSourceOrientation()
    {
        lastEulerOfAmbisonicSource = this.transform.eulerAngles;
        /* Ambisonic 音源の方向を回転させる */
        RoatateAmbisonicSourceOrientationByTransformOfChild(ref lastEulerOfAmbisonicSource);
        atomEx3DsourceForAmbisonics.SetOrientation(
            ambisonicSourceOrientationFront,
            ambisonicSourceOrientationTop
            );
        atomEx3DsourceForAmbisonics.Update();
    }


    private void UpdateAmbisonicSourceOrientation()
    {
        /* Ambisonic Source の角度が変わっていたら Ambisonics 音源方向を更新 */
        if (lastEulerOfAmbisonicSource != this.transform.eulerAngles)
        {
            ForceUpdateAmbisonicSourceOrientation();
        }
    }


    private void RoatateAmbisonicSourceOrientationByTransformOfChild(ref Vector3 input_euler)
    {
        Quaternion quat = Quaternion.Euler(input_euler);
        float square_norm = quat.x * quat.x + quat.y * quat.y + quat.z * quat.z + quat.w * quat.w;
        float s;
        /* 正規化係数算出 */
        if (square_norm <= 0.0f)
        {
            s = 0.0f;
        }
        else
        {
            s = 2.0f / square_norm;
        }
        float[] matrix = new float[9];
        matrix[0] = 1.0f - s * (quat.y * quat.y + quat.z * quat.z);
        matrix[1] = s * (quat.x * quat.y - quat.w * quat.z);
        matrix[2] = s * (quat.x * quat.z + quat.w * quat.y);
        matrix[3] = s * (quat.x * quat.y + quat.w * quat.z);
        matrix[4] = 1.0f - s * (quat.x * quat.x + quat.z * quat.z);
        matrix[5] = s * (quat.y * quat.z - quat.w * quat.x);
        matrix[6] = s * (quat.x * quat.z - quat.w * quat.y);
        matrix[7] = s * (quat.y * quat.z + quat.w * quat.x);
        matrix[8] = 1.0f - s * (quat.x * quat.x + quat.y * quat.y);
        /* Matrix calculation */
        {
            /* the default orientation vector to front of atom 3d source */
            Vector3 default_front = new Vector3(0, 0, 1);
            Vector3 output_front = ambisonicSourceOrientationFront;
            output_front.x = matrix[0] * default_front.x + matrix[1] * default_front.y + matrix[2] * default_front.z;
            output_front.y = matrix[3] * default_front.x + matrix[4] * default_front.y + matrix[5] * default_front.z;
            output_front.z = matrix[6] * default_front.x + matrix[7] * default_front.y + matrix[8] * default_front.z;
            ambisonicSourceOrientationFront = output_front;
        }
        {
            /* the default orientation vector to top of atom 3d source */
            Vector3 default_top = new Vector3(0, 1, 0);
            Vector3 output_top = ambisonicSourceOrientationTop;
            output_top.x = matrix[0] * default_top.x + matrix[1] * default_top.y + matrix[2] * default_top.z;
            output_top.y = matrix[3] * default_top.x + matrix[4] * default_top.y + matrix[5] * default_top.z;
            output_top.z = matrix[6] * default_top.x + matrix[7] * default_top.y + matrix[8] * default_top.z;
            ambisonicSourceOrientationTop = output_top;
        }
    }
    #endregion
}

/**
 * @}
 */

/* end of file */
