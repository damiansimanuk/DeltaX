namespace ECommerce.App.Services;
using Namotion.Reflection;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using System.Reflection;

public sealed class AddAdditionalTypeProcessorAssembly : IDocumentProcessor
{
    private List<ContextualType> contextualTypes;

    public AddAdditionalTypeProcessorAssembly(params Assembly[] assemblies)
    {
        contextualTypes = assemblies
            .SelectMany(t => { try { return t.GetTypes(); } catch { return []; } })
            .Select(t => t.ToContextualType())
            .Distinct()
            .ToList();
    }

    public void Process(DocumentProcessorContext context)
    {
        foreach (var type in contextualTypes)
        {
            context.SchemaGenerator.Generate(type, context.SchemaResolver);
        }
    }
}