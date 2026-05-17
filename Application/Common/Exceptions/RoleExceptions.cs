namespace Application.Common.Exceptions;

public class RoleNotFoundException(int roleId)
    : BaseException(roleId, $"Role not found under id {roleId}");

public class RoleAlreadyExistsException(string name)
    : BaseException(0, $"Role with name {name} already exists");

public class UnhandledRoleException(int roleId, Exception? innerException = null)
    : BaseException(roleId, "Unhandled role exception", innerException);