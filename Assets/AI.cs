using NWH.VehiclePhysics;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    public AudioClip[] falasNarrativas;

    public AudioSource falas;

    public GameObject panelAI;

    [TextArea(3, 10)]
    public string[] combustivel50;

    [TextArea(3, 10)]
    public string[] combustivel0;

    private string sairDoDomo;

    private string avisotenda;

    private string[] narrativa;

    private string[] tutorialItens;

    private string[] bluePrints;

    private string[] avisoEsteira;

    private string areabloqueada;

    public float duraçãoDasFalas;

    public GameObject PanelGameOver;
    public GameObject PanelForaDoDomo;

    public Text text;

    public Dome domo;
    public Slider lifeIndicatorSld;
    public float maxLife = 60f;
    public GameObject anim;
    private float life;

    private bool falaAtiva;
    private bool novafala;
    private bool trocarfala;
    private int narrativaAtual;
   // private float timer;
    private float timerAtivarAsCercas;
    private int gravidadeAtingida;
    private bool aviso;
    private bool naoSaiDaTenda;

    private bool emAvisos;
    private bool danoInicial;
    // Start is called before the first frame update
    void Start()
    {
        danoInicial = true;
        narrativaAtual = 0;
       // timer = duraçãoDasFalas;
        timerAtivarAsCercas = 45;
        gravidadeAtingida = 14;

        life = 60;
        lifeIndicatorSld.maxValue = maxLife;
        lifeIndicatorSld.value = life;


        //////////////Falas
        narrativa = new string[52];

        narrativa[0] = "Aviso! Uma forte tempestade se aproxima. Marcando coordenadas de estruturas mais próximas para abrigo.";
        narrativa[1] = "A tempestade atingiu níveis críticos. Não é aconselhável continuar exposto.";
        narrativa[2] = "Os fortes ventos danificaram o S.N.C. É necessário acionar as cercas manualmente para evitar danos fatais. Marcando as assinaturas de energia no seu visor.";
        narrativa[3] = "Para isso, você terá que utilizar a plataforma veicular remota.";
        narrativa[4] = "Este é o protótipo remoto de coleta e exploração Scorpion-11, ele está equipado com uma garra hidráulica de alta performance.";
        narrativa[5] = "Seja rápido pois essa estrutura sofrerá colapso estrutural se a tempestade atingir nível 21 de gravidade...";
        narrativa[6] = "As noites nesse planeta são muito escuras. O acionamento dos faróis é recomendado!";
        narrativa[7] = "Sistemas ativados com sucesso. Todas as ameaças climáticas foram neutralizadas num raio de 200 metros. O nível total de energia do sistema de neutralização climática é: 15 %.";
        narrativa[8] = "Agora que você não está mais correndo risco de vida iminente. Meu nome é: K4-n137u-J0gg1 3B. Mas você pode me chamar de Liz.";
        narrativa[9] = "Eu fui programada para ser ativada assim que você chegasse à Gaya e estivesse em risco e vou te ajudar a completar sua missão e sobreviver.";
        narrativa[10] = "Inclusive... É preciso estabilizar o sistema com 100% da sua capacidade. Transfira energia para a unidade de distribuição energética.";
        narrativa[11] = "Essa é a Central de Distribuição de Energia, ou C.D.E. Ela pode armazenar energia por longos períodos e é compatível com qualquer equipamento.";
        narrativa[12] = "Use-a para abastecer as estruturas, veículos e equipamentos.";
        narrativa[13] = "É aconselhável a construção de um gerador para suprir as necessidades energéticas do complexo.";
        narrativa[14] = "Por sorte tenho a planta de um modelo arcaico em meu banco de dados, mas infelizmente os recursos necessários estão indisponíveis, então você terá que coletá - los você mesmo.";
        narrativa[15] = "As instruções estão em sua interface de sobrevivência.";
        narrativa[16] = "Você pode explorar o planeta o quanto achar necessário. Desde que tenha energia no veículo para isso. Você pode abastecer o seu veículo sempre que estiver na base.";
        narrativa[17] = "Tome cuidado com acelerar demais e subidas muito íngremes, pois a energia é consumida mais rápido nessas situações.";
        narrativa[18] = "Quando sua bateria estiver perto da metade eu vou lhe avisar para que saiba que é hora de voltar.";
        narrativa[19] = "Caso a energia do veículo acabe , todo e qualquer recurso coletado é ejetado para fora e o veículo retorna imediatamente para a base.";

        narrativa[20] = "Avarias podem ocorrer em decorrência da direção da I.A do veículo";

        narrativa[20] = "Todas as tecnologias de sobrevivência estão operando de acordo. Mantenha o S.N.C energizado ou você não poderá deixar a M.A.N.T.A sem o auxílio de um veículo até montá-lo e carregá-lo novamente.";
        narrativa[21] = "Transferindo coordenadas para seu mapa. Não esqueça de abastecer o veículo antes de ir.";
        narrativa[22] = "De acordo com as coordenadas, este é o lugar referido na última gravação do log de acesso pessoal.";
        narrativa[23] = "Análise médica - Batimentos e composição sanguínea indicam: Funções orgânicas normais.Funções mentais flutuando: Medo, dor, sofrimento, culpa.";
        narrativa[24] = "Escaneando artefato… Artefato escaneado: Natureza e fabricantes desconhecidos. É possível que a peça seja utilizada para filtragem.";
        narrativa[25] = "Funções mentais flutuando: Medo, dor, sofrimento, culpa, dúvida, angústia. Pressão sanguínea e batimentos caindo: 60bpm, 50bpm, 40bpm…";
        narrativa[26] = "A tecnologia do artefato é compatível com o Gerador de Biomassa! A emissão de C02 está caindo substancialmente!";
        narrativa[27] = "Inclusive… Seu veículo não tripulado está com as cargas quase esgotadas! Deixe-o na base de recarga.";
        narrativa[28] = "Enquanto seu veículo é recarregado, tenho algumas coisas pra te mostrar aqui no acampamento.";
        narrativa[29] = "Marquei no seu mapa o Sistema de Indução Energética do acampamento.";
        narrativa[30] = "Até agora as configurações do S.I.E estavam automáticas, mas essa função demanda mais energia.";
        narrativa[31] = "Você pode direcionar energia para os equipamentos da base manualmente a fim de economizar alguns amperes.";
        narrativa[32] = "Vamos voltar para a M.A.N.TA. Enquanto você explorava o vale, o sistema de mapeamento automático coletou várias informações úteis que você pode acessar de sua nave.";
        narrativa[33] = "O Sistema de Mapeamento por Sonda tem catalogado uma grande extensão do planeta, mas os detalhes específicos de cada área são imprecisos.";
        narrativa[34] = "Conforme você explora, o mapa é atualizado com novas informações e detalhes! Veja! Neste exato minuto o mapa está identificando uma frequência incomum não muito longe daqui.";
        narrativa[35] = "Frequência de tecnologia humana! Você pode marcar o local em seu mapa e as informações são transmitidas para sua bússola e interface!";
        narrativa[36] = "Enquanto você estava lá fora, coletei dados meteorológicos ultra precisos e transformei o Lobby de comunicação numa central de missão.";
        narrativa[37] = "Aqui você pode verificar eventos climáticos que estão para acontecer e avaliar a situação geral do planeta. Também é possível ver as informações coletadas da missão.";
        narrativa[38] = "Estou detectando uma vibração energética intensa e disforme. Não consigo gerar um relatório preciso sem uma análise visual. Suba na torre de observação e veja se encontra de onde vem essa energia!";
        narrativa[39] = "Enquanto você estava lá fora, coletei dados meteorológicos ultra precisos e transformei o Lobby de comunicação numa central de missão.";
        narrativa[40] = "Não consegui identificar o que estava produzindo toda essa energia, mas consegui rastrear de onde vinha. O problema é que a distância é grande e o caminho é difícil…";
        narrativa[41] = "Seu veículo não está equipado para transpor terreno tão exigente. Buscando plantas de modificação veicular…";
        narrativa[42] = "Pronto. Coletei uma série de planos de engenharia para você poder modificar seu veículo. Você só precisa se preocupar com ter os recursos necessários para construir as peças.";
        narrativa[43] = "A construção de pneus nível 2 e uma melhoria no motor são as principais recomendações no momento.";
        narrativa[44] = "Parece confiável o bastante! Mas não é bom lotar o veículo de peso. Quanto mais carga, mais consumo de energia e há chances de perder os recursos caso fique sem energia e o sistema de retorno de emergência seja acionado!";
        narrativa[45] = "Vou mostrar onde guardar o que não precisar por agora.";
        narrativa[46] = "Essas anomalias podem nos levar direto aos astronautas, mas o caminho é longo e difícil…";
        narrativa[47] = "Estou identificando frequências de tecnologia humana. Leitura inconclusiva, há forte interferência.";
        narrativa[48] = "Buscando formas de vida num raio de 1km…";
        narrativa[49] = "O resultado da busca foi: 0.";
        narrativa[50] = "Este é um modelo de placa condutora GJ-X2, mas foi modificada. Leve-a de volta ao acampamento para estudo detalhado.";
        narrativa[51] = "Os números apontam para um abandono veicular manual. O acidente tem 3,48721 % de chance de ter sido letal. Realize uma busca por novas pistas e retorne ao acampamento.";


        //////////////Tutorial para os itens
        tutorialItens = new string[15];

        tutorialItens[0] = "Assim como em outros planetas, a fauna local é ativa, então não será incomum encontrar alguns destes pelo caminho.";
        tutorialItens[1] = "Este em especial parece já estar seco, e apto para abastecer o gerador. Você pode coletá - lo com as mãos.";
        tutorialItens[2] = "Seu traje está equipado com um cortador de plasma expansível, use - o para separar a parte rochosa da mineral e assim coletar os materiais necessários.";
        tutorialItens[3] = "A rocha de tom amarronzado que você encontrará espalhada pelo planeta é bauxita.";
        tutorialItens[4] = "Você pode usar o refinador de bauxita para obter alumínio. Material bastante necessário para sua jornada nesse planeta.";
        tutorialItens[5] = "Já esta outra rocha contém ferro. O cortador de plasma dar conta de sua separação e não é necessário mais refinamento.";
        tutorialItens[6] = "Pelo planeta você também poderá encontrar ouro e cristal de silício. Sua coleta é recomendada.";
        tutorialItens[7] = "Assim como em outros planetas, aqui há seres de origem e tamanho singulares. Colônias de bactérias bio-luminescentes podem ser vistas pelo planeta inteiro.";
        tutorialItens[8] = "Quando submetidas ao ambiente apropriado, elas produzem plástico. Para conseguir uma amostra, use seu tubo de pesquisa e análise.";
        tutorialItens[9] = "Quando as bactérias são depositadas nessa máquina depois de algumas horas elas produzem plástico que será muito útil futuramente.";
        tutorialItens[10] = "Leituras de energia detectadas: Parece que este objeto não pertence a nós. Segundo o banco de dados há mais destes espalhados pelo vale, mas não há registros de sua função.";
        tutorialItens[11] = "Um forte pulso eletromagnético foi detectado! Leituras indicam um fluxo de energia constante ao redor do núcleo. Parece que seu sistema de indução é compatível com nossa tecnologia!";
        tutorialItens[12] = "Carga da bateria do veículo subindo com velocidade surpreendente. É recomendável buscar mais destes objetos pelo planeta!";
        tutorialItens[13] = "Alguns dos recursos você encontra na natureza, outros não. Você pode usar o Conversor de Recursos para obter subprodutos dos recursos naturais.";
        tutorialItens[14] = "Essa ação demanda energia e é impossível desfazê-la, então tome cuidado para não desperdiçar tempo e recursos!";


        //////////////Falas da BluePrint
        bluePrints = new string[3];

        bluePrints[0] = "Parece que você encontrou um plano de engenharia! A frequência devia estar vindo daqui… Parece uma esteira. Útil para terreno acidentado.";
        bluePrints[1] = "Vou transferir as informações para a central da oficina.";
        bluePrints[2] = "Seu VTNT pode carregar até 3 propulsores diferentes e é possível alterna-los em qualquer lugar! Você pode acessar esses controles pela interface de inventário.";

        //////////////Aviso para não sair do Domo
        sairDoDomo = "As condições climáticas do planeta não são favoráveis à vida humana. Utilize os veículos para explorar além da proteção do S.N.C.";

        //////////////Aviso para não sair da Tenda
        avisotenda = "As condições climáticas são extremamente perigosas! Para explorar é necessário o uso do veículo.";

        //////////////Aviso da esteira
        avisoEsteira = new string[2];

        avisoEsteira[0] = "Este terreno é acidentado demais… O consumo de energia está em níveis muito altos! Seria recomendado propulsores adaptados para esse terreno.";
        avisoEsteira[1] = "Quando estiver em terreno acidentado, troque para a esteira! Assim terá melhor desempenho e menor consumo. Mas cuidado, em terreno comum a esteira é lenta e consome muita energia!";

        //////////////Aviso áreas bloqueadas
        areabloqueada = "As tempestades impossibilitaram traçar rota por esses caminhos… Bom, pelo menos por terra. De acordo com meus cálculos em 22 horas o trânsito será possível novamente.";
    }



    // Update is called once per frame
    void Update()
    {
        if(panelAI.activeSelf)
        {
            if(!falas.isPlaying)
            {
                CancelarFala();
            }
        }

        if (novafala)
        {
            anim.SetActive(false);
            anim.SetActive(true);
            falaAtiva = true;
            novafala = false;
            trocarfala = true;

        }

        if (falaAtiva)
        {

            if (!panelAI.activeSelf || trocarfala)
            {
                panelAI.SetActive(true);
                falas.clip = falasNarrativas[narrativaAtual];
                falas.Play();
                anim.SetActive(true);
                text.text = narrativa[narrativaAtual].ToUpper();
                trocarfala = false;
                narrativaAtual++;
            }

        }

        else
        {
            if (aviso)
            {

                if (!panelAI.activeSelf)
                {
                    anim.SetActive(true);
                    panelAI.SetActive(true);
                    text.text = sairDoDomo.ToUpper();
                    aviso = false;
                    emAvisos = true;
                }

                else
                {
                    anim.SetActive(true);
                    text.text = sairDoDomo.ToUpper();
                    aviso = false;
                    emAvisos = true;
                }

            }

            if (naoSaiDaTenda)
            {
                if (!panelAI.activeSelf)
                {
                    anim.SetActive(true);
                    panelAI.SetActive(true);
                    text.text = avisotenda.ToUpper();
                    naoSaiDaTenda = false;
                    emAvisos = true;
                }

                else
                {
                    anim.SetActive(true);
                    text.text = avisotenda.ToUpper();
                    naoSaiDaTenda = false;
                    emAvisos = true;
                }
            }

        }



        if (narrativaAtual == 3 && falaAtiva == false)
        {

            if (!panelAI.activeSelf && timerAtivarAsCercas >= 40)
            {
                panelAI.SetActive(true);
                anim.SetActive(true);
                text.text = "Nível " + gravidadeAtingida + " Atingido".ToUpper(); ;
            }


            timerAtivarAsCercas -= Time.deltaTime;

            if (timerAtivarAsCercas <= 0)
            {
                anim.SetActive(true);
                gravidadeAtingida++;
                text.text = "Nível " + gravidadeAtingida + " Atingido".ToUpper(); ;
                timerAtivarAsCercas = 45;
            }

            if (gravidadeAtingida == 21)
            {
                PanelGameOver.SetActive(true);
            }

        }



        ///*******Vida 0O Player

        if (!Cheats.imortal)
        {


                 if (domo.GetForaDoDomo() || danoInicial)
                {
                   life -= Time.timeScale * 0.1f;
                   lifeIndicatorSld.value = life;
                }

                else if (!domo.GetForaDoDomo() && life < maxLife  )
                {
                    if (life > maxLife)
                    {
                        life = maxLife;
                    }

                    life += Time.timeScale * 0.1f;
                    lifeIndicatorSld.value = life;
                }

                if (life <= 0)
                {
                    PanelForaDoDomo.SetActive(true);
                }


        }
    }

    public void SetFalaAtiva(bool state)
    {
        novafala = state;
    }

    public bool GetFalaAtiva()
    {
        return falaAtiva;
    }

    public void SetDanoInicial(bool inicio)
    {
        danoInicial = inicio;
    }

    public void Avisinho()
    {
        aviso = true;
    }

    public int GetnarrativaAtual()
    {
        return narrativaAtual;
    }

    public void SetnarrativaAtual(int narrativa)
    {
        narrativaAtual = narrativa;
    }

    public void CancelarFala()
    {
        if (falaAtiva)
        {
            panelAI.SetActive(false);
            falaAtiva = false;
        }

        
        else if(emAvisos)
        { 
            panelAI.SetActive(false);
            emAvisos = false;
        }

        if (narrativaAtual == 3 && falaAtiva == false)
        {
            panelAI.SetActive(false);
        }

    }

    public void NaoSaiDaTenda()
    {
        naoSaiDaTenda = true;
    }

}
