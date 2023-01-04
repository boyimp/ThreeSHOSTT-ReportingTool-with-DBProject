Create VIEW [dbo].[HHTAutomationCommands] as

(
  SELECT a.Id AutomationCommandId, a.Name Name,  r.ID UserRoleId, r.Name UserRoleName, ButtonHeader, 
  CAST(con.ParameterValues AS NVARCHAR(4000))ParameterValues, 'Order' OperationLine, EnableStates = m.EnabledStates, m.VisibleStates
  FROM 
  AutomationCommands a, AutomationCommandMaps m, AppRules ar, AppRuleMaps amap, ActionContainers con,
  (
    (SELECT 0 Id, 'ALL' Name)
    UNION 
    SELECT Id, Name FROM UserRoles 
  )
  r
  WHERE 
  a.Id = m.AutomationCommandId
  AND r.Id = m.UserRoleId 
  AND m.DisplayOnOrders = 1
  AND ar.eventname = 'AutomationCommandExecuted'
  AND amap.UserRoleId = r.Id
  AND amap.AppRuleID = ar.id
  AND CAST(ar.eventconstraints AS NVARCHAR(1000)) = 'AutomationCommandName;=;' + a.Name 
  AND con.Appruleid = ar.id
  AND con.Name = 'Update Order'
)
UNION 
(
  SELECT a.Id AutomationCommandId, a.Name Name,  r.ID UserRoleId, r.Name UserRoleName, ButtonHeader, 
  CAST('IsLocket=1' AS NVARCHAR(4000))ParameterValues, 'Ticket' OperationLine, m.EnabledStates, m.VisibleStates
  FROM 
  AutomationCommands a, AutomationCommandMaps m, AppRules ar, AppRuleMaps amap, ActionContainers con, AppActions ac,
  (
   (SELECT 0 Id, 'ALL' Name)
   UNION 
   SELECT Id, Name FROM UserRoles 
  )
  r
  WHERE 
  a.Id = m.AutomationCommandId
  AND r.Id = m.UserRoleId  
  AND ar.eventname = 'AutomationCommandExecuted'
  AND amap.UserRoleId = r.Id
  AND amap.AppRuleID = ar.id
  AND CAST(ar.eventconstraints AS NVARCHAR(1000)) = 'AutomationCommandName;=;' + a.Name 
  AND con.Appruleid = ar.id 
  AND con.appactionid = ac.Id
  AND ac.actiontype = 'ExecutePrintJob'
);

GO

