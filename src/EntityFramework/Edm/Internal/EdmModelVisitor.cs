namespace System.Data.Entity.Edm.Internal
{
    using System.Collections.Generic;

    internal abstract class EdmModelVisitor : DataModelItemVisitor
    {
        protected virtual void VisitEdmDataModelItem(EdmDataModelItem item)
        {
        }

        protected virtual void VisitEdmMetadataItem(EdmMetadataItem item)
        {
            VisitEdmDataModelItem(item);
            if (item != null)
            {
                if (item.HasAnnotations)
                {
                    VisitAnnotations(item, item.Annotations);
                }
            }
        }

        protected virtual void VisitEdmNamedMetadataItem(EdmNamedMetadataItem item)
        {
            VisitEdmMetadataItem(item);
        }

        protected virtual void VisitEdmNamespaceItem(EdmNamespaceItem item)
        {
            VisitEdmNamedMetadataItem(item);
        }

        #region Container, EntitySet, AssociationSet, FunctionImport

        protected virtual void VisitEntityContainers(EdmModel model, IEnumerable<EdmEntityContainer> entityContainers)
        {
            VisitCollection(entityContainers, VisitEdmEntityContainer);
        }

        protected virtual void VisitEdmEntityContainer(EdmEntityContainer item)
        {
            VisitEdmNamedMetadataItem(item);
            if (item != null)
            {
                if (item.HasEntitySets)
                {
                    VisitEntitySets(item, item.EntitySets);
                }

                if (item.HasAssociationSets)
                {
                    VisitAssociationSets(item, item.AssociationSets);
                }

#if IncludeUnusedEdmCode
                if (item.HasFunctionImports)
                {
                    this.VisitFunctionImports(item, item.FunctionImports);
                }
#endif
            }
        }

        protected virtual void VisitEntitySets(EdmEntityContainer container, IEnumerable<EdmEntitySet> entitySets)
        {
            VisitCollection(entitySets, VisitEdmEntitySet);
        }

        protected virtual void VisitEdmEntitySet(EdmEntitySet item)
        {
            VisitEdmNamedMetadataItem(item);
        }

        protected virtual void VisitAssociationSets(
            EdmEntityContainer container, IEnumerable<EdmAssociationSet> associationSets)
        {
            VisitCollection(associationSets, VisitEdmAssociationSet);
        }

        protected virtual void VisitEdmAssociationSet(EdmAssociationSet item)
        {
            VisitEdmNamedMetadataItem(item);
            if (item.SourceSet != null)
            {
                VisitEdmAssociationSetEnd(item.SourceSet);
            }
            if (item.TargetSet != null)
            {
                VisitEdmAssociationSetEnd(item.TargetSet);
            }
        }

        protected virtual void VisitEdmAssociationSetEnd(EdmEntitySet item)
        {
            VisitEdmNamedMetadataItem(item);
        }

#if IncludeUnusedEdmCode
        protected virtual void VisitFunctionImports(EdmEntityContainer container, IEnumerable<EdmFunctionImport> functionImports)
        {
            VisitCollection(functionImports, this.VisitFunctionImport);
        }

        protected virtual void VisitFunctionImport(EdmFunctionImport item)
        {
            this.VisitEdmNamedMetadataItem(item);
            if (item.ReturnType != null)
            {
                this.VisitEdmTypeReference(item.ReturnType);
            }

            this.VisitFunctionImportParameters(item, item.Parameters);
        }

        protected virtual void VisitFunctionImportParameters(EdmFunctionImport item, IEnumerable<EdmFunctionParameter> parameters)
        {
            VisitCollection(item.Parameters, this.VisitFunctionParameter);
        }
#endif

        #endregion

        #region Namespace, EntityType, ComplexType, Association, Function

        protected virtual void VisitNamespaces(EdmModel model, IEnumerable<EdmNamespace> namespaces)
        {
            VisitCollection(namespaces, VisitEdmNamespace);
        }

        protected virtual void VisitEdmNamespace(EdmNamespace item)
        {
            VisitEdmNamedMetadataItem(item);
            if (item != null)
            {
                if (item.HasComplexTypes)
                {
                    VisitComplexTypes(item, item.ComplexTypes);
                }

                if (item.HasEntityTypes)
                {
                    VisitEntityTypes(item, item.EntityTypes);
                }

                if (item.HasEnumTypes)
                {
                    VisitEnumTypes(item, item.EnumTypes);
                }

                if (item.HasAssociationTypes)
                {
                    VisitAssociationTypes(item, item.AssociationTypes);
                }

#if IncludeUnusedEdmCode
                if (item.HasFunctionGroups)
                {
                    this.VisitFunctionGroups(item, item.FunctionGroups);
                }
#endif
            }
        }

        protected virtual void VisitComplexTypes(EdmNamespace edmNamespace, IEnumerable<EdmComplexType> complexTypes)
        {
            VisitCollection(complexTypes, VisitComplexType);
        }

        protected virtual void VisitComplexType(EdmComplexType item)
        {
            VisitEdmNamedMetadataItem(item);
            if (item.HasDeclaredProperties)
            {
                VisitCollection(item.DeclaredProperties, VisitEdmProperty);
            }
        }

        protected virtual void VisitDeclaredProperties(EdmComplexType complexType, IEnumerable<EdmProperty> properties)
        {
            VisitCollection(properties, VisitEdmProperty);
        }

        protected virtual void VisitEntityTypes(EdmNamespace edmNamespace, IEnumerable<EdmEntityType> entityTypes)
        {
            VisitCollection(entityTypes, VisitEdmEntityType);
        }

        protected virtual void VisitEnumTypes(EdmNamespace edmNamespace, IEnumerable<EdmEnumType> enumTypes)
        {
            VisitCollection(enumTypes, VisitEdmEnumType);
        }

        protected virtual void VisitEdmEnumType(EdmEnumType item)
        {
            VisitEdmNamedMetadataItem(item);
            if (item != null)
            {
                if (item.HasMembers)
                {
                    VisitEnumMembers(item, item.Members);
                }
            }
        }

        protected virtual void VisitEnumMembers(EdmEnumType enumType, IEnumerable<EdmEnumTypeMember> members)
        {
            VisitCollection(members, VisitEdmEnumTypeMember);
        }

        protected virtual void VisitEdmEntityType(EdmEntityType item)
        {
            VisitEdmNamedMetadataItem(item);
            if (item != null)
            {
                if (item.HasDeclaredKeyProperties)
                {
                    VisitDeclaredKeyProperties(item, item.DeclaredKeyProperties);
                }

                if (item.HasDeclaredProperties)
                {
                    VisitDeclaredProperties(item, item.DeclaredProperties);
                }

                if (item.HasDeclaredNavigationProperties)
                {
                    VisitDeclaredNavigationProperties(item, item.DeclaredNavigationProperties);
                }
            }
        }

        protected virtual void VisitDeclaredKeyProperties(EdmEntityType entityType, IEnumerable<EdmProperty> properties)
        {
            VisitCollection(properties, VisitEdmProperty);
        }

        protected virtual void VisitDeclaredProperties(EdmEntityType entityType, IEnumerable<EdmProperty> properties)
        {
            VisitCollection(properties, VisitEdmProperty);
        }

        protected virtual void VisitDeclaredNavigationProperties(
            EdmEntityType entityType, IEnumerable<EdmNavigationProperty> navigationProperties)
        {
            VisitCollection(navigationProperties, VisitEdmNavigationProperty);
        }

        protected virtual void VisitAssociationTypes(
            EdmNamespace edmNamespace, IEnumerable<EdmAssociationType> associationTypes)
        {
            VisitCollection(associationTypes, VisitEdmAssociationType);
        }

        protected virtual void VisitEdmAssociationType(EdmAssociationType item)
        {
            VisitEdmNamedMetadataItem(item);

            if (item != null)
            {
                if (item.SourceEnd != null)
                {
                    VisitEdmAssociationEnd(item.SourceEnd);
                }
                if (item.TargetEnd != null)
                {
                    VisitEdmAssociationEnd(item.TargetEnd);
                }
            }
            if (item.Constraint != null)
            {
                VisitEdmAssociationConstraint(item.Constraint);
            }
        }

#if IncludeUnusedEdmCode

        protected virtual void VisitFunctionGroups(EdmNamespace edmNamespace, IEnumerable<EdmFunctionGroup> functionGroups)
        {
            VisitCollection(functionGroups, this.VisitEdmFunctionGroup);
        }

        protected virtual void VisitEdmFunctionGroup(EdmFunctionGroup item)
        {
            this.VisitEdmNamespaceItem(item);
            if (item != null)
            {
                if(item.HasOverloads)
                {
                    VisitFunctionOverloads(item, item.Overloads);
                }
            }
        }

        protected virtual void VisitFunctionOverloads(EdmFunctionGroup functionGroup, IEnumerable<EdmFunctionOverload> functionOverloads)
        {
            VisitCollection(functionOverloads, this.VisitEdmFunctionOverload);
        }

        protected virtual void VisitEdmFunctionOverload(EdmFunctionOverload item)
        {
            VisitEdmMetadataItem(item);
            if(item != null)
            {
                if (item.ReturnType != null)
                {
                    this.VisitEdmTypeReference(item.ReturnType);
                }

                if(item.HasParameters)
                {
                    this.VisitFunctionOverloadParameters(item, item.Parameters);
                }
            }
        }
#endif

        protected virtual void VisitEdmTypeReference(EdmTypeReference reference)
        {
            VisitEdmMetadataItem(reference);
            if (reference.HasFacets)
            {
                VisitEdmPrimitiveTypeFacets(reference.PrimitiveTypeFacets);
            }
        }

        protected virtual void VisitEdmPrimitiveTypeFacets(EdmPrimitiveTypeFacets facets)
        {
            VisitEdmDataModelItem(facets);
        }

        protected virtual void VisitEdmProperty(EdmProperty item)
        {
            VisitEdmNamedMetadataItem(item);
            if (item.PropertyType != null)
            {
                VisitEdmTypeReference(item.PropertyType);
            }
        }

        protected virtual void VisitEdmEnumTypeMember(EdmEnumTypeMember item)
        {
            VisitEdmNamedMetadataItem(item);
        }

        protected virtual void VisitEdmAssociationEnd(EdmAssociationEnd item)
        {
            VisitEdmNamedMetadataItem(item);
        }

        protected virtual void VisitEdmAssociationConstraint(EdmAssociationConstraint item)
        {
            if (item != null)
            {
                VisitEdmMetadataItem(item);
                if (item.DependentEnd != null)
                {
                    VisitEdmAssociationEnd(item.DependentEnd);
                }
                VisitCollection(item.DependentProperties, VisitEdmProperty);
            }
        }

        protected virtual void VisitEdmNavigationProperty(EdmNavigationProperty item)
        {
            VisitEdmNamedMetadataItem(item);
        }

#if IncludeUnusedEdmCode
        protected virtual void VisitFunctionOverloadParameters(EdmFunctionOverload functionOverload, IEnumerable<EdmFunctionParameter> functionParameters)
        {
            VisitCollection(functionParameters, this.VisitFunctionParameter);
        }

        protected virtual void VisitFunctionParameter(EdmFunctionParameter item)
        {
            this.VisitEdmNamedMetadataItem(item);
            if (item.ParameterType != null)
            {
                this.VisitEdmTypeReference(item.ParameterType);
            }
        }
#endif

        #endregion

        protected virtual void VisitEdmModel(EdmModel item)
        {
            VisitEdmNamedMetadataItem(item);
            if (item != null)
            {
                if (item.HasNamespaces)
                {
                    VisitNamespaces(item, item.Namespaces);
                }

                if (item.HasContainers)
                {
                    VisitEntityContainers(item, item.Containers);
                }
            }
        }
    }
}
