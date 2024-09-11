// See https://aka.ms/new-console-template for more information
using DeltaX.ResultFluent;
using DemoBlazor.Shared.Contracts;
using DemoBlazor.Shared.Entities;
using NJsonSchema;
using NJsonSchema.Generation;


//var jsonSchemaGenerator = new JSchemaGenerator();
//var schema = jsonSchemaGenerator.Generate(typeof(CreateTourRequest));


//var result = schema.ToString();

//Console.WriteLine(result);

//JsonSchemaGeneratorSettings.GenerateAbstractProperties

//JsonSchemaGenerator.FromType

var settings = new SystemTextJsonSchemaGeneratorSettings()
{
    //GenerateAbstractSchemas = true,
    //GenerateAbstractProperties = true,
    //AlwaysAllowAdditionalObjectProperties = true,
    //AllowReferencesWithProperties = true,
    //FlattenInheritanceHierarchy = false,
};


var schema = new JsonSchema();
var resolver = new JsonSchemaResolver(schema, settings);
var generator = new JsonSchemaGenerator(settings);
generator.Generate(typeof(TourDto), resolver);
generator.Generate(typeof(Result), resolver);
generator.Generate(typeof(Result<TourDto>), resolver);
generator.Generate(schema, typeof(Service), resolver);

var schemaData = schema.ToJson();
Console.WriteLine(schemaData);

schema = await NJsonSchema.JsonSchema.FromJsonAsync(schemaData);


Console.ReadKey();