using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Survival_Manager : MonoBehaviour
{
    [Header("Modelos 3D - Base")]
    public GameObject[] modelosBase;

    [Header("Modelos 3D - Planeta")]
    public GameObject[] modelosPlaneta;

    [Header("Infos - Base")]
    public GameObject[] infosBase;

    [Header("Infos - Planeta")]
    public GameObject[]infosPlaneta;

    //Estados do menu
    private string PLANETA = "Planeta";
    private string BASE = "Base";
    private string ARTEFATOS = "Artefatos";
    private string VEICULO = "VEICULO";
    public string estado;

    [Header("Botoes Menu")]
    public Button botao_base;
    public Button botao_veiculo;
    public Button botao_planeta;
    //public Button botao_artefatos;
    public bool botao_base_selected;
    bool botao_veiculo_selected;
    bool botao_planeta_selected;

    //paineis menu
    public GameObject base_panel;
    public GameObject planeta_panel;
    public GameObject artefato_panel;

    public GameObject conjunto_3d_planeta;
    public GameObject conjunto_3d_base;
    public GameObject conjunto_info_planeta;
    public GameObject conjunto_info_base;
    public GameObject conjunto_info_artefato;
    public GameObject conjunto_3d_artefato;

    //array menu - BASE
    private int itens_base;
    //array menu - Planeta
    private int itens_planeta;

    void Start()
    {   //estado = BASE;
        itens_base = 1;
        itens_planeta = 1;
        botao_base.Select();
        botao_base_selected = true;

        modelosBase[0].SetActive(true);
    
    }

  
    void Update()
    {
        //Botoes_Base();
        //Botoes_Planeta();
        Buttons_UI();

        Debug.Log(botao_veiculo_selected);
    }

    public void Botoes_Planeta()
    {
        if (itens_planeta < 1)
            itens_planeta = 1;

        if (itens_planeta > modelosPlaneta.Length)
            itens_planeta = modelosPlaneta.Length;

        for (int i = 0; i < modelosPlaneta.Length; i++)
        {
            if (i == itens_planeta - 1)
            {
                modelosPlaneta[i].SetActive(true);
                infosPlaneta[i].SetActive(true);
            }

            else
            {
                modelosPlaneta[i].SetActive(false);
                infosPlaneta[i].SetActive(false);
            }
        }
    }

    public void Botoes_Base()
    {
        if (itens_base < 1)
            itens_base = 1;

        if (itens_base > modelosBase.Length)
            itens_base = modelosBase.Length;

        for (int i = 0; i < modelosBase.Length; i++)
        {
            if (i == itens_base-1)
            {
                modelosBase[i].SetActive(true);
                infosBase[i].SetActive(true);
            }

            else
            {
                modelosBase[i].SetActive(false);
                infosBase[i].SetActive(false);
            }
        }
    }

    public void Buttons_UI ()
    {
        if (botao_veiculo_selected)
            botao_base.Select();
        
    }

    public void Selected_Button_Base()
    {
        botao_base_selected = true;
        botao_planeta_selected = false;
        botao_veiculo_selected = false;
    }
    public void Selected_Button_Veiculo()
    {
        //botao_base_selected = false;
        botao_planeta_selected = false;
        botao_veiculo_selected = true;
    }
    public void Selected_Planeta_Base()
    {
        //botao_base_selected = false;
        botao_planeta_selected = true;
        botao_veiculo_selected = false;
    }

    //botoes de fwd e bckwds base
    public void FWD_Base ()
    {
        itens_base += 1;
        Botoes_Base();
    }

    public void BWD_Base()
    {
        itens_base -= 1;
        Botoes_Base();
    }
    //botoes de fwd e bckwds planeta
    public void FWD_Planeta()
    {
        itens_planeta += 1;
        Botoes_Planeta();
    }

    public void BWD_Planeta()
    {
        itens_planeta -= 1;
        Botoes_Planeta();
    }
    //botoes Gerais --
    public void Planeta_Selected ()
    {
        artefato_panel.SetActive(false);
        planeta_panel.SetActive(true);
        base_panel.SetActive(false);
        conjunto_3d_planeta.SetActive(true);
        conjunto_3d_base.SetActive(false);
        conjunto_info_base.SetActive(false);
        conjunto_info_planeta.SetActive(true);
        conjunto_3d_artefato.SetActive(false);
        conjunto_info_artefato.SetActive(false);
    }
    public void Base_Selected()
    {
        artefato_panel.SetActive(false);
        planeta_panel.SetActive(true);
        base_panel.SetActive(false);
        conjunto_3d_base.SetActive(true);
        conjunto_3d_planeta.SetActive(false);
        conjunto_info_base.SetActive(true);
        conjunto_info_planeta.SetActive(false);

        conjunto_3d_artefato.SetActive(false);
        conjunto_info_artefato.SetActive(false);
    }
    public void Artefato_Selected ()
    {
        planeta_panel.SetActive(false);
        base_panel.SetActive(false);

        artefato_panel.SetActive(true);
        conjunto_3d_artefato.SetActive(true);
        conjunto_info_artefato.SetActive(true);

        conjunto_3d_base.SetActive(false);
        conjunto_3d_planeta.SetActive(false);
        conjunto_info_base.SetActive(false);
        conjunto_info_planeta.SetActive(false);
    }


    //ESTADOS PARA OS OnClick()
    public void Estado_Veiculo()
    {
        estado = VEICULO;
        //ativa painel e 3d veiculo e etc...
    }
    public void Estado_Base()
    {
        estado = BASE;
    }
    public void Estado_Planeta()
    {
        estado = PLANETA;
    }
    public void Estado_Artefatos()
    {
        estado = ARTEFATOS;
    }
}
