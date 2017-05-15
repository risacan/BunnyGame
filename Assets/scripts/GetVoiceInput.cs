using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GetVoiceInput : MonoBehaviour {
    public GameObject textObject;
    private AudioSource audio_;
    private float[] frequencyData;
    private int nSamples = 1024;
    private float fMax;

    void Start()
    {
        audio_ = GetComponent<AudioSource>();
        frequencyData = new float[nSamples];
        fMax = AudioSettings.outputSampleRate / 2;

    }

    public float BandVol(float fLow, float fHigh) {
        fLow = Mathf.Clamp(fLow, 20, fMax);
        fHigh = Mathf.Clamp(fHigh, fLow, fMax);
        audio_.GetSpectrumData(frequencyData, 0, FFTWindow.BlackmanHarris);
        var n1 = Mathf.Floor(fLow * nSamples / fMax);
        var n2 = Mathf.Floor(fHigh * nSamples / fMax);
        float sum = 0;
        int nn1 = (int) n1;
        int nn2 = (int) n2;

        for (int i = nn1; i <= nn2; i ++) {
            sum += frequencyData[i];
        }

        return sum / (n2 - n1 + 1);
    }

    void Update() {
        var spectrum = audio_.GetSpectrumData(1024, 0, FFTWindow.BlackmanHarris);
        for (int i = 1; i < spectrum.Length - 1; ++i) {
            // 対数y軸
            /*
            Debug.DrawLine(
                    new Vector3(i - 1, spectrum[i] + 10, 0),
                    new Vector3(i, spectrum[i + 1] + 10, 0),
                    Color.red);
            Debug.DrawLine(
                    new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2),
                    new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2),
                    Color.cyan);
            */

            // 対数x軸
            Debug.DrawLine(
                    new Vector3(Mathf.Log(i - 1)*(float)3, spectrum[i - 1] - 10, 1),
                    new Vector3(Mathf.Log(i)*(float)3, spectrum[i] - 10, 1),
                    Color.green);
            Debug.DrawLine(
                    new Vector3(Mathf.Log(i - 1)*(float)3, Mathf.Log(spectrum[i - 1]), 3),
                    new Vector3(Mathf.Log(i)*(float)3, Mathf.Log(spectrum[i]), 3),
                    Color.yellow);
            Debug.DrawLine(
                    new Vector3(Mathf.Log(i - 1)*(float)3, -5.0f, 2),
                    new Vector3(Mathf.Log(i)*(float)3, -5.0f, 2),
                    Color.blue
            );
            Debug.DrawLine(
                    new Vector3(10.0f, 5.0f, 2),
                    new Vector3(10.0f, -10.0f, 2),
                    Color.blue
            );

            if ( Mathf.Log(spectrum[i - 1]) > -5.0f && i < 10.0f) {
                textObject.GetComponent<Text>().text = "You are !";
            }
        }

    }

    public void PushButton() {
        // 空の Audio Sourceを取得
        // Audio Source の Audio Clip をマイク入力に設定
        // 引数は、デバイス名（null ならデフォルト）、ループ、何秒取るか、サンプリング周波数
        // audio_.clip = Microphone.Start(null, true, 1, 44100);
        // recordedClip = audio_.clip;
        // // マイクが Ready になるまで待機（一瞬）
        // while (Microphone.GetPosition(null) <= 0) {}
        // // 再生開始（録った先から再生、スピーカーから出力するとハウリングします）
    }
}
