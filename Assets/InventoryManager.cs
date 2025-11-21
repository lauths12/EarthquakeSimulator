using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public int maxItems = 7;
    private List<string> collectedItems = new List<string>();

    public TextMeshProUGUI inventoryText;

    // -------------------------
    // PUNTAJES SEPARADOS
    // -------------------------
    public int itemScore = 0;      // Puntos por objetos (Día 1)
    public float zoneScoreFloat = 0f; // Puntos de zonas (float real)
    public int zoneScore = 0;      // Convertido solo para visual

    public bool DisablePicking { get; set; } = false;

    private float lastPickupTime = 0f;
    private float minInterval = 0.2f;

    private readonly string[] displayTags = { "Food", "Water", "Medicine", "Tools" };

    private readonly Dictionary<string, string> tagSymbols = new Dictionary<string, string>
    {
        { "Food", " [HP] "},
        { "Water", " [H2O] " },
        { "Medicine", " [MED] " },
        { "Tools", " [TOOL] " }
    };

    void Start()
    {
        UpdateUI();
    }

    // ======================================================
    // 1️⃣ SUMA OBJETO Y RECALCULA PUNTAJE (DÍA 1)
    // ======================================================
    public bool AddItem(string tag, GameObject obj)
    {
        if (DisablePicking) return false;

        if (Time.time - lastPickupTime < minInterval)
            return false;

        lastPickupTime = Time.time;

        if (collectedItems.Count >= maxItems)
            return false;

        collectedItems.Add(tag);

        // Nuevo cálculo completo del puntaje por inventario
        RecalculateItemScore();

        UpdateUI();
        return true;
    }

    // ======================================================
    // LÓGICA DE PUNTAJE PARA INVENTARIO (DÍA 1)
    // ======================================================
    private void RecalculateItemScore()
    {
        int foodCount = collectedItems.Count(t => t == "Food");
        int waterCount = collectedItems.Count(t => t == "Water");
        int medicineCount = collectedItems.Count(t => t == "Medicine");
        int toolsCount = collectedItems.Count(t => t == "Tools");

        int usedSlots = collectedItems.Count;
        int remainingSlots = maxItems - usedSlots;

        int score = 0;

        // ------------------------------
        // VALORES BASE
        // ------------------------------
        score += foodCount * 5;
        score += waterCount * 15;
        score += medicineCount * 20;
        score += toolsCount * 8;

        // ------------------------------
        // BONOS
        // ------------------------------
        if (foodCount > 0) score += 5;
        if (waterCount > 0) score += 10;
        if (medicineCount > 0) score += 10;
        if (toolsCount > 0) score += 5;

        // ------------------------------
        // PENALIZACIONES (NUEVA LÓGICA)
        // ------------------------------

        // Penaliza exceso de comida siempre
        if (foodCount > 2)
            score -= (foodCount - 2) * 5;

        // ---------------------------------------------
        // Penalizaciones por NO llevar agua/medicina:
        // SOLO se aplican cuando ya NO se puede corregir
        // ---------------------------------------------

        // Faltan agua y medicina al mismo tiempo
        bool missingWater = waterCount == 0;
        bool missingMedicine = medicineCount == 0;

        if (missingWater && missingMedicine)
        {
            // No hay slots suficientes para llevar ambas
            if (remainingSlots < 2)
            {
                score -= 25; // penalización por faltar ambos
            }
        }
        else
        {
            // Falta solo uno (agua o medicina)
            // Aplica penalización si ya no queda espacio
            if (remainingSlots == 0)
            {
                if (missingWater) score -= 15;
                if (missingMedicine) score -= 10;
            }
        }

        // ❌ Se permiten negativos
        itemScore = score;
    }


    // ======================================================
    // 2️⃣ SUMA/RESTA POR ZONAS (DÍA 2) — AHORA PERMITE NEGATIVOS
    // ======================================================
    public void ModifyScoreContinuous(float amount)
    {
        zoneScoreFloat += amount; // acumula reales con negativos

        zoneScore = Mathf.RoundToInt(zoneScoreFloat);

        UpdateUI();
    }

    // ======================================================
    // 3️⃣ UI
    // ======================================================
    private void UpdateUI()
    {
        if (inventoryText == null) return;

        int finalScore = itemScore + Mathf.RoundToInt(zoneScoreFloat);

        // -------------------------------------------------
        // FUNCIONES PARA COLORES SEGÚN SIGNO
        // -------------------------------------------------
        string ColorFor(int value)
        {
            if (value > 0) return "#00FF00";   // verde
            if (value < 0) return "#FF4444";   // rojo
            return "#000000";             // negro
        }

        string itemColor = ColorFor(itemScore);
        string zoneColor = ColorFor(zoneScore);
        string totalColor = ColorFor(finalScore);

        // -------------------------------------------------
        // UI STRING
        // -------------------------------------------------
        string uiString = "";

        uiString += "<size=120%><color=#87CEEB><b>-- INVENTARIO ACTIVO --</b></color></size>\n";

        // Puntajes separados (CON COLORES DINÁMICOS)
        uiString += $"<size=105%><color={itemColor}><b>Puntaje Objetos: {itemScore}</b></color></size>\n";
        uiString += $"<size=105%><color={zoneColor}><b>Puntaje Zonas: {zoneScore}</b></color></size>\n";

        // Puntaje total (CON COLOR DINÁMICO)
        uiString += $"<size=120%><color={totalColor}><b>PUNTAJE TOTAL: {finalScore}</b></color></size>\n";

        uiString += "<size=70%>============================</size>\n";

        // Slots
        uiString += $"<size=90%>Slots Usados: <b>{collectedItems.Count}</b> / {maxItems}</size>\n";
        uiString += "<size=70%>============================</size>\n";

        // Advertencias
        if (collectedItems.Count >= maxItems)
        {
            uiString += "<size=100%><color=#FF0000><b>!!! NO CABEN MÁS ELEMENTOS !!!</b></color></size>\n";
            uiString += "<size=70%>============================</size>\n";
        }
        else if (DisablePicking)
        {
            uiString += "<size=100%><color=#FF0000><b>!!! TIEMPO AGOTADO !!!</b></color></size>\n";
            uiString += "<size=70%>============================</size>\n";
        }

        // Lista de objetos
        foreach (string tag in displayTags)
        {
            int count = collectedItems.Count(t => t == tag);
            string symbol = tagSymbols[tag];

            uiString += $"<size=100%>{symbol} <b>{tag}:</b><pos=70%><color=#228B22><b>{count}</b></color></pos></size>\n";
        }

        inventoryText.text = uiString;
    }

}
