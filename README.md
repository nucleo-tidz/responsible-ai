# 🛡️ Azure Content Safety Integration with Semantic Kernel

This sample demonstrates how to use **Azure Content Safety** to filter LLM interactions in a **Semantic Kernel**-powered application. It shows how to implement a `PromptRenderFilter` to block unsafe inputs or outputs based on:

- ✅ Built-in safety categories: `Sexual`, `Self-Harm`, `Hate`, `Violence`
- 🔧 Custom safety category support (e.g., personal-data , internal policy )

> 📌 Technologies Used:  
> - .NET 8 / C#  
> - Azure Content Safety API  
> - Microsoft Semantic Kernel  
> - PromptRenderFilter (for input/output filtering)

---

## 🧠 How It Works

When a user prompt is sent to the LLM (like GPT-4), the app:

1. Intercepts the prompt using a `PromptRenderFilter`
2. Calls Azure Content Safety API to check the prompt
3. Blocks or allows the request based on severity thresholds
4. Supports out-of-the-box categories and custom categories simultaneously

---
## 🛠️ Setting Up a Custom Category in Azure Content Safety

To enforce organization-specific safety filters (e.g., personal-data or sensitive project names), you can create and train a **Custom Category** in Azure Content Safety.

Follow these steps:

1. **Go to Azure AI Foundry**  
   Navigate to [Azure AI Studio](https://ai.azure.com/) or your Azure AI resource in the Azure Portal.

2. **Open `Guardrails + Controls`**  
   From the left-hand navigation pane, click **Guardrails + Controls**.

3. **Select the `Custom Categories` tab**  
   Scroll to the bottom and click the **Create custom category** button.

4. **Add New Category Details**  
   - Click **Add new category**
   - Provide a **Name** (e.g., `personal-data`)
   - Add a **Description**
   - Enter a **public URL** pointing to your **`.jsonl` training file** stored in Azure Blob Storage , [Sample data](https://github.com/nucleo-tidz/responsible-ai/tree/main/trainingdata)
     > grant Data Reader and Contributor roles for both Azure AI Foundry and Azure Content Safety services
5. **Click `Create and Train`**  
   Azure will begin training your category. This may take a few minutes.


