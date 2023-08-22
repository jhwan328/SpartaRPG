using SpartaRPG.Managers;
using static SpartaRPG.Managers.SceneManager;

internal class Program
{

    static void Main(string[] args)
    {
        GameManager.Instance.DataManager.GameDataSetting();

        while (true)
        {
            GameManager.Instance.SceneManager.LoadScene();
        }
    }
}

