---@class EffectExecutor
local SpellExecutor = class("SpellExecutor")

function SpellExecutor:ctor(sess)
    self.sess = sess
end

function SpellExecutor:ExecuteSpell()
end

return SpellExecutor