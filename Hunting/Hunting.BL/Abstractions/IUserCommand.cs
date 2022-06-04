﻿using Hunting.BL.Enum;

namespace Hunting.BL.Abstractions;

public interface IUserCommand<in TParams> : ICommand<TParams, UserCommandExecutionResult> where TParams : IContract
{ }