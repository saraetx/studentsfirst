using System;
using System.Collections.Generic;
using BlazorState;
using StudentsFirst.Spa.Wasm.Features.Groups.ViewModels;

namespace StudentsFirst.Spa.Wasm.Features.Groups
{
    public partial class GroupsState : State<GroupsState>
    {
        public IDictionary<string, GroupViewModel>? AvailableGroups;

        public override void Initialize() { }
    }
}
